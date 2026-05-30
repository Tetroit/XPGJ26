#ifndef UNITY_PI
#define UNITY_PI 3.14159265358979323846
#endif

float3 BlendNorm(float3 n1, float3 n2)
{
	return normalize(float3(n1.xy * n2.z + n2.xy * n1.z, n1.z * n2.z));
}

void GerstnerWave_float(
	float time,
	float2 pos,
	float2 offset,
	float amplitude, 
	float wavelength, 
	float speed,
	float dir,
	out float3 outPos,
	out float3 outDispl,
	out float3 outNorm,
	out float3 outTan,
	out float3 outBinorm)
{
	float k = 2 * UNITY_PI / wavelength;
	float2 d = normalize( float2(cos(dir), sin(dir)));
	float f = k * (dot(d, pos + offset) - speed * time);
	outDispl.x = amplitude * cos(f) * d.x;
	outDispl.y = amplitude * sin(f);
	outDispl.z = amplitude * cos(f) * d.y;
	outPos.x = pos.x + outDispl.x;
	outPos.y = outDispl.y;
	outPos.z = pos.y + outDispl.z;
	
	outTan.x = 1 - amplitude * k * d.x * d.x * sin(f);
	outTan.y = amplitude * k * d.x * cos(f);
	outTan.z = -amplitude * k * d.x * d.y * sin(f);

	outBinorm.x = - amplitude * k * d.x * d.y * sin(f);
	outBinorm.y = amplitude * k * d.y * cos(f);
	outBinorm.z = 1 - amplitude * k * d.y * d.y * sin(f);
	
	outNorm = normalize(cross(outBinorm, outTan));
}

void MultiGerstner_float(
	float time,
	float2 pos,
	float2 offset,
	float amplitude,
	float wavelength, 
	float speed,
	float dir,
	out float3 outPos,
	out float3 outNorm)
{
	float3 n = float3(0, 1, 0);
	float3 p = float3(pos.x, 0, pos.y);
	float3 disp = float3(0, 0, 0);
	float3 tan = float3(1, 0, 0);
	float3 binorm = float3(0, 0, 1);
	
	for (int i = 0; i < 5; i++)
	{
 		float3 norm1, pos1, dis1, tan1, bin1 ;
 		GerstnerWave_float(time, pos, offset, amplitude, wavelength, speed, dir, pos1, dis1, norm1, tan1, bin1);
		disp += dis1;
		tan += tan1;
		binorm += bin1;
		
 		amplitude *= 0.67f;
 		wavelength *= 0.66f;
 		dir += 1.2f;
	}
	outPos = p + disp;
	outNorm = normalize(cross(binorm, tan));
}