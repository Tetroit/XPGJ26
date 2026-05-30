using Unity.VisualScripting;
using UnityEngine;

public class WaveGen : MonoBehaviour
{
    [SerializeField]
    private Material waterMat;
    [SerializeField]
    private float wavelength;
    [SerializeField]
    private float amplitude;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float direction;
    [SerializeField]
    private float wavelengthReduction;
    [SerializeField]
    private float amplitudeReduction;
    [SerializeField]
    private float directionRotation;
    [SerializeField]
    private Vector2 linearVelocity;

    void Awake()
    {
        ApplyToMaterial();
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        for (int x = 0; x < 5; x++)
        {
            for (int y = 0; y < 5; y++)
            {
                var res = SampleGerstner(Time.time, new Vector2(x, y));
                Gizmos.DrawWireSphere(res.position, 0.1f);
                Gizmos.DrawLine(new Vector3(x, 0, y), res.position);
            }
        }
    }
    struct WaveInfo
    {
        public Vector3 offset;
        public Vector3 position;
        public Vector3 tangent;
        public Vector3 binormal;
        public Vector3 normal;
    }
    WaveInfo SampleWaveInfo(float time, Vector2 coord, Vector2 offset, float waveL, float ampl, float dir)
    {
        WaveInfo info = new WaveInfo();
        float k = 2 * Mathf.PI / waveL;
        Vector2 d = Vector2.Normalize(new Vector2(Mathf.Cos(dir), Mathf.Sin(dir)));
        float f = k * (Vector2.Dot(d, coord + offset) - speed * time);
        info.offset.x = ampl * Mathf.Cos(f) * d.x;
        info.offset.y = ampl * Mathf.Sin(f);
        info.offset.z = ampl * Mathf.Cos(f) * d.y;
        info.position.x = coord.x + info.offset.x;
        info.position.y = info.offset.y;
        info.position.z = coord.y + info.offset.z;
	
        info.tangent.x = 1 - ampl * k * d.x * d.x * Mathf.Sin(f);
        info.tangent.y = ampl * k * d.x * Mathf.Cos(f);
        info.tangent.z = -ampl * k * d.x * d.y * Mathf.Sin(f);

        info.binormal.x = - ampl * k * d.x * d.y * Mathf.Sin(f);
        info.binormal.y = ampl * k * d.y * Mathf.Cos(f);
        info.binormal.z = 1 - ampl * k * d.y * d.y * Mathf.Sin(f);

        info.normal = Vector3.Normalize(Vector3.Cross(info.binormal, info.tangent));
        return info;
    }
    public struct MultiWaveInfo
    {
        public Vector3 position;
        public Vector3 normal;
        public Vector3 offset;
    }
    public MultiWaveInfo SampleGerstner(float time, Vector2 coord)
    {
        MultiWaveInfo info = new MultiWaveInfo();
        Vector3 n = new Vector3(0, 1, 0);
        Vector3 p = new Vector3(coord.x, 0, coord.y);
        Vector3 disp = new Vector3(0, 0, 0);
        Vector3 tan = new Vector3(1, 0, 0);
        Vector3 binorm = new Vector3(0, 0, 1);
        float dir = direction;
        float waveL = wavelength;
        float ampl = amplitude;

        Vector2 offsetMap = time * linearVelocity;
        Vector3 offset = new Vector3(offsetMap.x,  0, offsetMap.y);
        
        for (int i = 0; i < 5; i++)
        {
            Vector3 norm1, pos1, dis1, tan1, bin1 ;
            var subSample = SampleWaveInfo(time, coord, offsetMap, waveL, ampl, dir);
            disp += subSample.offset;
            tan += subSample.tangent;
            binorm += subSample.binormal;
		
            ampl *= amplitudeReduction;
            waveL *= wavelengthReduction;
            dir += directionRotation;
        }
        info.position = disp + p;
        info.normal = Vector3.Normalize(Vector3.Cross(binorm, tan));
        info.offset = disp;
        return info;
    }
    [ContextMenu("Update Material")]
    void ApplyToMaterial()
    {
        if (waterMat == null)
            return;
        waterMat.SetFloat("_Wavelength", wavelength);
        waterMat.SetFloat("_Amplitude", amplitude);
        waterMat.SetFloat("_Speed", speed);
        waterMat.SetFloat("_Direction", direction);
        waterMat.SetVector("_Linear_Velocity", linearVelocity);
        waterMat.SetFloat("_WavMult", wavelengthReduction);
        waterMat.SetFloat("_AmpMult", amplitudeReduction);
        waterMat.SetFloat("_MultiRot", directionRotation);
    }
}
