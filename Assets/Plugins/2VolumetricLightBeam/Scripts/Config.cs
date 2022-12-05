﻿#if UNITY_EDITOR
//#define PROFILE_INSTANCE_LOADING
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Serialization;

namespace VLB
{
    [HelpURL(Consts.Help.UrlConfig)]
    public class Config : ScriptableObject
    {
        public const string ClassName = "Config";

        public const string kAssetName = "VLBConfigOverride";
        public const string kAssetNameExt = ".asset";

        /// <summary>
        /// Override the layer on which the procedural geometry is created or not
        /// </summary>
        public bool geometryOverrideLayer = Consts.Config.GeometryOverrideLayerDefault;

        /// <summary>
        /// The layer the procedural geometry gameObject is in (only if geometryOverrideLayer is enabled)
        /// </summary>
        public int geometryLayerID = Consts.Config.GeometryLayerIDDefault;

        /// <summary>
        /// The tag applied on the procedural geometry gameObject
        /// </summary>
        public string geometryTag = Consts.Config.GeometryTagDefault;

        /// <summary>
        /// Determine in which order beams are rendered compared to other objects.
        /// This way for example transparent objects are rendered after opaque objects, and so on.
        /// </summary>
        public int geometryRenderQueue = (int)Consts.Config.GeometryRenderQueueDefault;
        public int geometryRenderQueueHD = (int)Consts.Config.HD.GeometryRenderQueueDefault;

        /// <summary>
        /// Select the Render Pipeline (Built-In or SRP) in use.
        /// </summary>
        public RenderPipeline renderPipeline
        {
            get { return m_RenderPipeline; }
            set
            {
#if UNITY_EDITOR
                m_RenderPipeline = value;
#else
                Debug.LogError("Modifying the RenderPipeline in standalone builds is not permitted");
#endif
            }
        }
        [FormerlySerializedAs("renderPipeline"), FormerlySerializedAs("_RenderPipeline")]
        [SerializeField] RenderPipeline m_RenderPipeline = Consts.Config.GeometryRenderPipelineDefault;

        /// <summary>
        /// MultiPass: Use the 2 pass shader. Will generate 2 drawcalls per beam.
        /// SinglePass: Use the 1 pass shader. Will generate 1 drawcall per beam.
        /// GPUInstancing: Dynamically batch multiple beams to combine and reduce draw calls (Feature only supported in Unity 5.6 or above). More info: https://docs.unity3d.com/Manual/GPUInstancing.html
        /// SRPBatcher: Use the SRP Batcher to automatically batch multiple beams and reduce draw calls. Only available when using SRP.
        /// </summary>
        public RenderingMode renderingMode
        {
            get { return m_RenderingMode; }
            set
            {
#if UNITY_EDITOR
                m_RenderingMode = value;
#else
                Debug.LogError("Modifying the RenderingMode in standalone builds is not permitted");
#endif
            }
        }
        [FormerlySerializedAs("renderingMode"), FormerlySerializedAs("_RenderingMode")]
        [SerializeField] RenderingMode m_RenderingMode = Consts.Config.GeometryRenderingModeDefault;


        public bool IsSRPBatcherSupported()
        {
            // The SRP Batcher Rendering Mode is only compatible when using a SRP
            if (renderPipeline == RenderPipeline.BuiltIn) return false;

            // SRP Batcher only works with URP and HDRP
            var rp = SRPHelper.projectRenderPipeline;
            return rp == RenderPipeline.URP || rp == RenderPipeline.HDRP;
        }

        /// <summary>
        /// Actual Rendering Mode used on the current platform
        /// </summary>
        public RenderingMode GetActualRenderingMode(ShaderMode shaderMode)
        {
            if (renderingMode == RenderingMode.SRPBatcher && !IsSRPBatcherSupported()) return RenderingMode.Default;

            // Using a Scriptable Render Pipeline with 'Multi-Pass' Rendering Mode is not supported
            if (renderPipeline != RenderPipeline.BuiltIn && renderingMode == RenderingMode.MultiPass) return RenderingMode.Default;

            // HD beams require single pass shaders
            if (shaderMode == ShaderMode.HD && renderingMode == RenderingMode.MultiPass) return RenderingMode.Default;

            return renderingMode;
        }

        /// <summary>
        /// Depending on the actual Rendering Mode used, returns true if the single pass shader will be used, false otherwise.
        /// </summary>
        public bool SD_useSinglePassShader { get { return GetActualRenderingMode(ShaderMode.SD) != RenderingMode.MultiPass; } }

        public bool SD_requiresDoubleSidedMesh { get { return SD_useSinglePassShader; } }

        /// <summary>
        /// Main shader applied to the cone beam geometry
        /// </summary>
        public Shader GetBeamShader(ShaderMode mode)
        {
#if UNITY_EDITOR
            var shader = GetBeamShaderInternal(mode);
            if (shader == null)
                RefreshShader(mode, RefreshShaderFlags.All);
            return shader;
#else
            return GetBeamShaderInternal(mode);
#endif
        }

        ref Shader GetBeamShaderInternal(ShaderMode mode)
        {
            if(mode == ShaderMode.SD)   return ref _BeamShader;
            else                        return ref _BeamShaderHD;
        }

        int GetRenderQueueInternal(ShaderMode mode)
        {
            if (mode == ShaderMode.SD)  return geometryRenderQueue;
            else                        return geometryRenderQueueHD;
        }

        public Material NewMaterialTransient(ShaderMode mode, bool gpuInstanced)
        {
            var material = MaterialManager.NewMaterialPersistent(GetBeamShader(mode), gpuInstanced);
            if (material)
            {
                material.hideFlags = Consts.Internal.ProceduralObjectsHideFlags;
                material.renderQueue = GetRenderQueueInternal(mode);
            }
            return material;
        }

        /// <summary>
        /// Depending on the quality of your screen, you might see some artifacts with high contrast visual (like a white beam over a black background).
        /// These is a very common problem known as color banding.
        /// To help with this issue, the plugin offers a Dithering factor: it smooths the banding by introducing a subtle pattern of noise.
        /// </summary>
        public float ditheringFactor = Consts.Config.DitheringFactor;

        /// <summary>
        /// Contribution of the attached spotlight temperature to the final beam color.
        /// Only useful when:
        /// - The beams is attached to a Unity spotlight.
        /// - The beams color is linked to the Unity Light color.
        /// - The Unity light uses 'color temperature mode' and is specified with 'Filter' and 'Temperature' properties.
        /// </summary>
        public bool useLightColorTemperature = Consts.Config.UseLightColorTemperatureDefault;

        /// <summary>
        /// Number of Sides of the shared cone mesh
        /// </summary>
        public int sharedMeshSides = Consts.Config.SharedMeshSidesDefault;

        /// <summary>
        /// Number of Segments of the shared cone mesh
        /// </summary>
        public int sharedMeshSegments = Consts.Config.SharedMeshSegmentsDefault;

        /// <summary>
        /// Distance from the camera the beam will fade (for HD beams only, for SD beams, this option can be configured per beam)
        /// 0 = hard intersection
        /// Higher values produce soft intersection when the camera is near the cone triangles.
        /// </summary>
        public float hdBeamsCameraBlendingDistance = Consts.Config.HD.CameraBlendingDistance;

        /// <summary>
        /// Global 3D Noise texture scaling: higher scale make the noise more visible, but potentially less realistic.
        /// </summary>
        [Range(Consts.Beam.NoiseScaleMin, Consts.Beam.NoiseScaleMax)]
        public float globalNoiseScale = Consts.Beam.NoiseScaleDefault;

        /// <summary>
        /// Global World Space direction and speed of the noise scrolling, simulating the fog/smoke movement
        /// </summary>
        public Vector3 globalNoiseVelocity = Consts.Beam.NoiseVelocityDefault;

        /// <summary>
        /// Tag used to retrieve the camera used to compute the fade out factor on beams
        /// </summary>
        public string fadeOutCameraTag = Consts.Config.FadeOutCameraTagDefault;

        public Transform fadeOutCameraTransform
        {
            get
            {
                if (m_CachedFadeOutCamera == null)
                {
                    ForceUpdateFadeOutCamera();
                }

                return m_CachedFadeOutCamera;
            }
        }

        /// <summary>
        /// Call this function if you want to manually change the fadeOutCameraTag property at runtime
        /// </summary>
        public void ForceUpdateFadeOutCamera()
        {
            var gao = GameObject.FindGameObjectWithTag(fadeOutCameraTag);
            if (gao)
                m_CachedFadeOutCamera = gao.transform;
        }

        /// <summary>
        /// 3D Texture storing noise data.
        /// </summary>
        [HighlightNull]
        public Texture3D noiseTexture3D = null;

        /// <summary>
        /// ParticleSystem prefab instantiated for the Volumetric Dust Particles feature (Unity 5.5 or above)
        /// </summary>
        [HighlightNull]
        public ParticleSystem dustParticlesPrefab = null;

        /// <summary>
        /// Noise texture for dithering feature
        /// </summary>
        [HighlightNull]
        public Texture2D ditheringNoiseTexture = null;

        [HighlightNull]
        public Texture2D jitteringNoiseTexture = null;

        /// <summary>
        /// Off: do not support having a gradient as color.
        /// High Only: support gradient color only for devices with Shader Level = 35 or higher.
        /// High and Low: support gradient color for all devices.
        /// </summary>
        public FeatureEnabledColorGradient featureEnabledColorGradient = Consts.Config.FeatureEnabledColorGradientDefault;

        /// <summary>
        /// Support 'Soft Intersection with Opaque Geometry' feature or not.
        /// </summary>
        public bool featureEnabledDepthBlend = Consts.Config.FeatureEnabledDefault;

        /// <summary>
        /// Support 'Noise 3D' feature or not.
        /// </summary>
        public bool featureEnabledNoise3D = Consts.Config.FeatureEnabledDefault;

        /// <summary>
        /// Support 'Dynamic Occlusion' features or not.
        /// </summary>
        public bool featureEnabledDynamicOcclusion = Consts.Config.FeatureEnabledDefault;

        /// <summary>
        /// Support 'Mesh Skewing' feature or not.
        /// </summary>
        public bool featureEnabledMeshSkewing = Consts.Config.FeatureEnabledDefault;

        /// <summary>
        /// Support 'Shader Accuracy' property set to 'High' or not.
        /// </summary>
        public bool featureEnabledShaderAccuracyHigh = Consts.Config.FeatureEnabledDefault;

        /// <summary>
        /// Support 'Shadow' features or not.
        /// </summary>
        public bool featureEnabledShadow = true;

        /// <summary>
        /// Support 'Cookie' feature or not.
        /// </summary>
        public bool featureEnabledCookie = true;



        /// RAYMARCHING BEGIN
        [SerializeField] RaymarchingQuality[] m_RaymarchingQualities = null;

        [SerializeField] int m_DefaultRaymarchingQualityUniqueID = 0;

        public int defaultRaymarchingQualityUniqueID => m_DefaultRaymarchingQualityUniqueID;

        public RaymarchingQuality GetRaymarchingQualityForIndex(int index)
        {
            Debug.Assert(index >= 0);
            Debug.Assert(m_RaymarchingQualities != null);
            Debug.Assert(index < m_RaymarchingQualities.Length);
            return m_RaymarchingQualities[index];
        }

        public RaymarchingQuality GetRaymarchingQualityForUniqueID(int id)
        {
            int index = GetRaymarchingQualityIndexForUniqueID(id);
            if (index >= 0)
                return GetRaymarchingQualityForIndex(index);
            return null;
        }

        public int GetRaymarchingQualityIndexForUniqueID(int id)
        {
            for (int i = 0; i < m_RaymarchingQualities.Length; ++i)
            {
                var qual = m_RaymarchingQualities[i];
                if (qual != null && qual.uniqueID == id)
                    return i;
            }

            Debug.LogErrorFormat("Failed to find RaymarchingQualityIndex for Unique ID {0}", id);
            return -1;
        }

        public bool IsRaymarchingQualityUniqueIDValid(int id) { return GetRaymarchingQualityIndexForUniqueID(id) >= 0; }

#if UNITY_EDITOR
        public void AddRaymarchingQuality(RaymarchingQuality qual)
        {
            ArrayUtility.Add(ref m_RaymarchingQualities, qual);
        }

        public void RemoveRaymarchingQualityAtIndex(int index)
        {
            Debug.Assert(index >= 0);
            Debug.Assert(index < m_RaymarchingQualities.Length);
            ArrayUtility.RemoveAt(ref m_RaymarchingQualities, index);
        }
#endif

        public int raymarchingQualitiesCount { get { return Mathf.Max(1, m_RaymarchingQualities != null ? m_RaymarchingQualities.Length : 1); } }

        void CreateDefaultRaymarchingQualityPreset(bool onlyIfNeeded)
        {
            if (m_RaymarchingQualities == null || m_RaymarchingQualities.Length == 0 || !onlyIfNeeded)
            {
                m_RaymarchingQualities = new RaymarchingQuality[3];
                // set forced unique ID for default qualities to keep finding them even when deleting Config instance
                m_RaymarchingQualities[0] = RaymarchingQuality.New("Fast", 1, 5);
                m_RaymarchingQualities[1] = RaymarchingQuality.New("Balanced", 2, 10);
                m_RaymarchingQualities[2] = RaymarchingQuality.New("High", 3, 20);
                m_DefaultRaymarchingQualityUniqueID = m_RaymarchingQualities[1].uniqueID;
            }
        }
        /// RAYMARCHING END


        public bool isHDRPExposureWeightSupported
        {
            get
            {
            #if UNITY_2021_1_OR_NEWER
                return renderPipeline == RenderPipeline.HDRP;
            #else
                return false; // GetCurrentExposureMultiplier is accessible but doesn't return a proper value in Unity 2020 for some reasons
            #endif
            }
        }

        // INTERNAL
#pragma warning disable 0414
        [SerializeField] int pluginVersion = -1;
        [SerializeField] Material _DummyMaterial = null;
#pragma warning restore 0414

        [SerializeField] Shader _BeamShader = null;
        [SerializeField] Shader _BeamShaderHD = null;

        Transform m_CachedFadeOutCamera = null;

        public bool hasRenderPipelineMismatch { get { return (SRPHelper.projectRenderPipeline == RenderPipeline.BuiltIn) != (m_RenderPipeline == RenderPipeline.BuiltIn); } }

        [RuntimeInitializeOnLoadMethod]
        static void OnStartup()
        {
            Instance.m_CachedFadeOutCamera = null;
            Instance.RefreshGlobalShaderProperties();

#if UNITY_EDITOR
            Instance.RefreshShaders(RefreshShaderFlags.All);
#endif

            if (Instance.hasRenderPipelineMismatch)
                Debug.LogError("It looks like the 'Render Pipeline' is not correctly set in the config. Please make sure to select the proper value depending on your pipeline in use.", Instance);
        }

        public void Reset()
        {
            geometryOverrideLayer = Consts.Config.GeometryOverrideLayerDefault;
            geometryLayerID = Consts.Config.GeometryLayerIDDefault;
            geometryTag = Consts.Config.GeometryTagDefault;
            geometryRenderQueue = (int)Consts.Config.GeometryRenderQueueDefault;
            geometryRenderQueueHD = (int)Consts.Config.HD.GeometryRenderQueueDefault;

            sharedMeshSides = Consts.Config.SharedMeshSidesDefault;
            sharedMeshSegments = Consts.Config.SharedMeshSegmentsDefault;

            globalNoiseScale = Consts.Beam.NoiseScaleDefault;
            globalNoiseVelocity = Consts.Beam.NoiseVelocityDefault;

            renderPipeline = Consts.Config.GeometryRenderPipelineDefault;
            renderingMode = Consts.Config.GeometryRenderingModeDefault;
            ditheringFactor = Consts.Config.DitheringFactor;
            useLightColorTemperature = Consts.Config.UseLightColorTemperatureDefault;

            fadeOutCameraTag = Consts.Config.FadeOutCameraTagDefault;

            featureEnabledColorGradient = Consts.Config.FeatureEnabledColorGradientDefault;
            featureEnabledDepthBlend = Consts.Config.FeatureEnabledDefault;
            featureEnabledNoise3D = Consts.Config.FeatureEnabledDefault;
            featureEnabledDynamicOcclusion = Consts.Config.FeatureEnabledDefault;
            featureEnabledMeshSkewing = Consts.Config.FeatureEnabledDefault;
            featureEnabledShaderAccuracyHigh = Consts.Config.FeatureEnabledDefault;

            hdBeamsCameraBlendingDistance = Consts.Config.HD.CameraBlendingDistance;

            CreateDefaultRaymarchingQualityPreset(onlyIfNeeded: false);

            ResetInternalData();

#if UNITY_EDITOR
            GlobalMeshSD.Destroy();
            EditorExtensions.SetAllMeshesDirty();
#endif
        }

        void RefreshGlobalShaderProperties()
        {
            Shader.SetGlobalFloat(ShaderProperties.GlobalUsesReversedZBuffer, SystemInfo.usesReversedZBuffer ? 1.0f : 0.0f);
            Shader.SetGlobalFloat(ShaderProperties.GlobalDitheringFactor, ditheringFactor);
            Shader.SetGlobalTexture(ShaderProperties.GlobalDitheringNoiseTex, ditheringNoiseTexture);

            Shader.SetGlobalFloat(ShaderProperties.HD.GlobalCameraBlendingDistance, hdBeamsCameraBlendingDistance);
            Shader.SetGlobalTexture(ShaderProperties.HD.GlobalJitteringNoiseTex, jitteringNoiseTexture);
        }

#if UNITY_EDITOR
        public void _EditorSetRenderingModeAndRefreshShader(RenderingMode mode)
        {
            renderingMode = mode;
            RefreshShaders(RefreshShaderFlags.All);
        }

        void OnValidate()
        {
            sharedMeshSides = Mathf.Clamp(sharedMeshSides, Consts.Config.SharedMeshSidesMin, Consts.Config.SharedMeshSidesMax);
            sharedMeshSegments = Mathf.Clamp(sharedMeshSegments, Consts.Config.SharedMeshSegmentsMin, Consts.Config.SharedMeshSegmentsMax);

            ditheringFactor = Mathf.Clamp01(ditheringFactor);

            hdBeamsCameraBlendingDistance = Mathf.Max(hdBeamsCameraBlendingDistance, 0f);
        }

        void AutoSelectRenderPipeline()
        {
            var newPipeline = SRPHelper.projectRenderPipeline;
            if (newPipeline != renderPipeline)
            {
                renderPipeline = newPipeline;
                EditorUtility.SetDirty(this); // make sure to save this property change
                RefreshShaders(RefreshShaderFlags.All);
            }
        }

        public static void EditorSelectInstance()
        {
            Selection.activeObject = Instance; // this will create the instance if it doesn't exist
            if (Selection.activeObject == null)
                Debug.LogError("Cannot find any Config resource");
        }

        [System.Flags]
        public enum RefreshShaderFlags
        {
            Reference = 1 << 1,
            Dummy = 1 << 2,
            All = Reference | Dummy,
        }

        public void RefreshShaders(RefreshShaderFlags flags)
        {
            foreach (ShaderMode shaderMode in System.Enum.GetValues(typeof(ShaderMode)))
                RefreshShader(shaderMode, flags);
        }

        public void RefreshShader(ShaderMode shaderMode, RefreshShaderFlags flags)
        {
            ref Shader shader = ref GetBeamShaderInternal(shaderMode);

            if (flags.HasFlag(RefreshShaderFlags.Reference))
            {
                var prevShader = shader;

                var configProps = new ShaderGenerator.ConfigProps
                {
                    renderPipeline = m_RenderPipeline,
                    renderingMode = GetActualRenderingMode(shaderMode),
                    dithering = ditheringFactor > 0.0f,
                    noise3D = featureEnabledNoise3D,
                    colorGradient = featureEnabledColorGradient,
                    depthBlend = featureEnabledDepthBlend,
                    dynamicOcclusion = featureEnabledDynamicOcclusion,
                    meshSkewing = featureEnabledMeshSkewing,
                    shaderAccuracyHigh = featureEnabledShaderAccuracyHigh,
                    cookie = featureEnabledCookie,
                    shadow = featureEnabledShadow,
                    raymarchingQualities = m_RaymarchingQualities
                };

                shader = ShaderGenerator.Generate(shaderMode, configProps);

                if (shader != prevShader)
                {
                    EditorUtility.SetDirty(this);
                }
            }

            if (flags.HasFlag(RefreshShaderFlags.Dummy) && shader != null)
            {
                bool gpuInstanced = GetActualRenderingMode(shaderMode) == RenderingMode.GPUInstancing;
                _DummyMaterial = DummyMaterial.Create(shaderMode, shader, gpuInstanced);
            }

            if (_DummyMaterial == null)
            {
                Debug.LogError("No dummy material referenced to VLB config, please try to reset this asset.", this);
            }

            RefreshGlobalShaderProperties();
        }
#endif // UNITY_EDITOR

        public void ResetInternalData()
        {
            noiseTexture3D = Resources.Load("Noise3D_64x64x64") as Texture3D;

            dustParticlesPrefab = Resources.Load("DustParticles", typeof(ParticleSystem)) as ParticleSystem;

            ditheringNoiseTexture = Resources.Load("VLBDitheringNoise", typeof(Texture2D)) as Texture2D;
            jitteringNoiseTexture = Resources.Load("VLBBlueNoise", typeof(Texture2D)) as Texture2D;

#if UNITY_EDITOR
            RefreshShaders(RefreshShaderFlags.All);
#endif
        }

        public ParticleSystem NewVolumetricDustParticles()
        {
            if (!dustParticlesPrefab)
            {
                if (Application.isPlaying)
                {
                    Debug.LogError("Failed to instantiate VolumetricDustParticles prefab.");
                }
                return null;
            }

            var instance = Instantiate(dustParticlesPrefab);
            instance.useAutoRandomSeed = false;
            instance.name = "Dust Particles";
            instance.gameObject.hideFlags = Consts.Internal.ProceduralObjectsHideFlags;
            instance.gameObject.SetActive(true);
            return instance;
        }

        void OnEnable()
        {
            CreateDefaultRaymarchingQualityPreset(onlyIfNeeded:true);

            HandleBackwardCompatibility(pluginVersion, Version.Current);
            pluginVersion = Version.Current;
        }

        void HandleBackwardCompatibility(int serializedVersion, int newVersion)
        {
            if (serializedVersion == -1) return;            // freshly new spawned config: nothing to do
            if (serializedVersion == newVersion) return;    // same version: nothing to do

#if UNITY_EDITOR
            if (serializedVersion < 1830)
            {
                AutoSelectRenderPipeline();
            }

            if (serializedVersion < 1950)
            {
                ResetInternalData(); // retrieve Noise3D texture converted from binary data to texture 3D asset in 1950
                EditorUtility.SetDirty(this); // make sure to save this property change
            }

            if (serializedVersion < 1980)
            {
                useLightColorTemperature = false; // light temperature support introduced in version 1980
                EditorUtility.SetDirty(this); // make sure to save this property change
            }

            if (serializedVersion < 20000)
            {
                ResetInternalData(); // retrieve Jittering Noise texture introduced in 20000
                EditorUtility.SetDirty(this); // make sure to save this property change
            }

            if (newVersion > serializedVersion)
            {
                // Import to keep, we have to regenerate the shader each time the plugin is updated
                RefreshShaders(RefreshShaderFlags.All);
            }
#endif
        }

        // Singleton management
        static Config ms_Instance = null;
        public static Config Instance { get { return GetInstance(true); } }

#if UNITY_EDITOR && VLB_DEBUG
        public struct Guard : System.IDisposable {
            public Guard(bool assert) {
                if (m_IsAccessing && assert) Debug.LogError("Circular loop in Config.Instance");
                m_IsAccessing = true;
            }

            public void Dispose() { m_IsAccessing = false; }
            static bool m_IsAccessing = false;
        }
#endif // UNITY_EDITOR && VLB_DEBUG

#if UNITY_EDITOR
        static bool ms_ShouldInvalidateCache = false;
        public Config()
        {
            ms_ShouldInvalidateCache = true; // new instance detected, force the cache to be refreshed
        }
#endif

        static Config LoadAssetInternal(string assetName)
        {
        #if PROFILE_INSTANCE_LOADING
            var startTime = EditorApplication.timeSinceStartup;
        #endif
            var instance = Resources.Load<Config>(assetName);
        #if PROFILE_INSTANCE_LOADING
            var totalTime = EditorApplication.timeSinceStartup - startTime;
            Debug.Log($"Loading {assetName} in {(int)(totalTime*1000)} ms");
        #endif
            return instance;
        }

        private static Config GetInstance(bool assertIfNotFound)
        {
        #if UNITY_EDITOR && VLB_DEBUG
            using (new Guard(true))
        #endif // UNITY_EDITOR && VLB_DEBUG
            {
                bool updateInstance = ms_Instance == null;
            #if UNITY_EDITOR
                updateInstance |= ms_ShouldInvalidateCache; // Force instance reloading when detecting Config asset changes
            #endif
                if (updateInstance)
                {
                #if UNITY_EDITOR
                    if (ms_IsCreatingInstance)
                    {
                        Debug.LogError(string.Format("Trying to access Config.Instance while creating it. Breaking before infinite loop."));
                        return null;
                    }
                #endif // UNITY_EDITOR

                    // Try to load the instance
                    {
                        var newInstance = LoadAssetInternal(kAssetName + PlatformHelper.GetCurrentPlatformSuffix());
                        if (newInstance == null) newInstance = LoadAssetInternal(kAssetName);

                    #if UNITY_EDITOR
                        if (newInstance && newInstance != ms_Instance)
                        {
                            ms_Instance = newInstance;
                            newInstance.RefreshGlobalShaderProperties(); // make sure noise textures are properly loaded as soon as the editor is started
                        }
                        ms_ShouldInvalidateCache = false;
                    #endif // UNITY_EDITOR

                        ms_Instance = newInstance;
                    }

                    if (ms_Instance == null)
                    {
                    #if UNITY_EDITOR
                        ms_IsCreatingInstance = true;
                        ms_Instance = CreateInstanceAsset();
                        ms_IsCreatingInstance = false;

                        ms_Instance.AutoSelectRenderPipeline();

                        if (Application.isPlaying)
                            ms_Instance.Reset(); // Reset is not automatically when instancing a ScriptableObject when in playmode
                    #endif // UNITY_EDITOR
                        Debug.Assert(!(assertIfNotFound && ms_Instance == null), string.Format("Can't find any resource of type '{0}'. Make sure you have a ScriptableObject of this type in a 'Resources' folder.", typeof(Config)));
                    }
                }
            }
            return ms_Instance;
        }

    #if UNITY_EDITOR
        static bool ms_IsCreatingInstance = false;

        public bool IsCurrentlyUsedInstance() { return Instance == this; }

        public bool HasValidAssetName()
        {
            if (name.IndexOf(kAssetName) != 0)
                return false;

            return PlatformHelper.IsValidPlatformSuffix(GetAssetSuffix());
        }

        public string GetAssetSuffix()
        {
            var fullname = name;
            var strToFind = kAssetName;
            if (fullname.IndexOf(strToFind) == 0) return fullname.Substring(strToFind.Length);
            else return "";
        }

        static void CreateFolderAndAsset(Object obj, string folderParent, string folderResources, string assetName)
        {
            if (!AssetDatabase.IsValidFolder(string.Format("{0}/{1}", folderParent, folderResources)))
                AssetDatabase.CreateFolder(folderParent, folderResources);

            CreateAsset(obj, string.Format("{0}/{1}/{2}", folderParent, folderResources, assetName));
        }

        public static void CreateAsset(Object obj, string fullPath)
        {
            AssetDatabase.CreateAsset(obj, fullPath);
            AssetDatabase.SaveAssets();
        }

        static Config CreateInstanceAsset()
        {
            var asset = CreateInstance<Config>();
            Debug.Assert(asset != null);
            CreateFolderAndAsset(asset, "Assets", "Resources", kAssetName + kAssetNameExt);
            return asset;
        }
    #endif // UNITY_EDITOR
    }
}
