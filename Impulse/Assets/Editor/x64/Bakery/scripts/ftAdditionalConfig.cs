using UnityEditor;
using UnityEngine;

public class ftAdditionalConfig
{
    // Affects texture import settings for lightmaps
    public const bool mipmapLightmaps = false;

    // Shader eval coeff * gaussian convolution coeff
    // ... replaced with more typical convolution coeffs
    // Used for legacy light probes
    public const float irradianceConvolutionL0 =       0.2820947917f;
    public const float irradianceConvolutionL1 =       0.32573500793527993f;//0.4886025119f * 0.7346029443286334f;
    public const float irradianceConvolutionL2_4_5_7 = 0.2731371076480198f;//0.29121293321402086f * 1.0925484306f;
    public const float irradianceConvolutionL2_6 =     0.07884789131313001f;//0.29121293321402086f * 0.3153915652f;
    public const float irradianceConvolutionL2_8 =     0.1365685538240099f;//0.29121293321402086f * 0.5462742153f;

    // Used for L1 light probes and volumes
    public const float convL0 = 1;
    public const float convL1 = 0.9f; // approx convolution

    // Use PNG instead of TGA for shadowmasks, directions and L1 maps
    public const bool preferPNG = false;
}
