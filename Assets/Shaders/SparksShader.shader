Shader "Renze/Sparks" {
    Properties {
        Sparks ("Amount of Sparks", int) = 5
        Brightness ("Brightness of Sparks", float) = 1.0

        SpeedFactor ("Speed Multiplier", float) = 1.5
        LengthFactor ("Length Multiplier", float) = 0.6
        GroupFactor ("Group Multiplier", float) = 1.0
        SpreadFactor ("Spread Multiplier", float) = 0.3
        MinAngle ("Minimum Angle", float) = 0.1
        RandFactor ("Random Factor", float) = 1.0
        Red ("Red Sparks", Range(0, 1)) = 1
    }
	SubShader {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent"}
        LOD 100
        Blend One One
        ZWrite Off
        Cull Back

        Pass {
            CGPROGRAM
           
            #pragma vertex vert
            #pragma fragment frag
          // make fog work
            #pragma multi_compile_fog
      
            #include "UnityCG.cginc"

            int Sparks;
            float Brightness;

            float SpeedFactor;
            float LengthFactor;
            float GroupFactor;
            float SpreadFactor;
            float MinAngle;
            float RandFactor;
            bool Red;

            //METHODS
            float3 sampleAngle(float u1){
                float r = sqrt(u1);
                return float3(r * 0.809017, -sqrt(1.0-u1), r*0.587785);
            }

            float rand(float2 co){
                return frac(sin(dot(co.xy, float2(12.9898, 78.233))) * 43758.5453);
            }

            float spread(float2 co){
                return frac(sin(dot(co.xy, float2(12.9898,78.233))) * 43758.5453);
            }

            float planeIntersection(float3 rPos, float3 rDir, float3 n){
                return -dot(n, rPos) / dot(rDir, n);
            }

            float cylinder(float3 pos, float3 dir, float len){
                float x = dot(pos, dir);
                return max(max(length(pos - dir * x) - 0.2, x), -x-len);
            }

            float4 Colour(float age){
                float f = 1.0 - age * 0.05;
                if(floor(Red) == 0){
                    return float4(0.2*f*f, 0.5*f*f+0.05, 0.5*f+0.4, min(f*2.0, 1.0));
                } else {
                    return float4(0.5*f+0.4, 0.5*f*f+0.05, 0.2*f*f, min(f*2.0, 1.0));
                }
            }

            float trace(float3 rpos, float3 rdir, float2 fragCoord){
                float sparkT = planeIntersection(rpos, rdir, float3(0.587785, 0.0, -0.809017));
                float floorT = -rpos.y / rdir.y;
                
                float4 colour = float4(0.0, 0.0, 0.0, rdir.y < 0.0 ? 1.0 : 0.0);
                float3 sparkColour = float3(0.0, 0.0, 0.0);
    
                float3 floorPos = rpos + rdir + floorT;
                float3 sparkPos = rpos + rdir + sparkT;

                float time = _Time * SpeedFactor;
                for(int i = 0; i < Sparks; i++){
                    //Calculate spark pos and velocity
                    float a = spread(float2(i, 1.0))*SpreadFactor+MinAngle;
                    float b = spread(float2(i, 3.0))*RandFactor;
                    float startTime = spread(float2(i, 5.0)) * GroupFactor;
                    float3 dir = sampleAngle(a) * 10;

                    float3 start = dir * (1.35 + b * 0.3);
                    float3 force = -start * 0.02 + float3(0.0, 1.2, 0.0);
                    float c = frac(time + startTime) * 20.0;
                    float3 oset = start * c + force * c * c * 0.5;
                    
                    float3 v = start + force * c;
                    float vel = length(v) * LengthFactor;
                    float3 vDir = normalize(-v);
                    float4 sc = Colour(c);

                    // Shade floor
                    if (rdir.y < 0.0){
                        float3 sPos = floorPos + oset;
                        float h = cylinder(sPos, vDir, vel);
                        
                        float invRad = 10.0;
                        float dist = h * 0.05;
                        float atten = 1.0 / (1.0 + 2.0 * invRad * dist + invRad * invRad * dist * dist);
                        if(floorT <= sparkT && sparkT > 0.0){
                            dist += 0.8;
                            atten += 1.0 / (1.0 + 100.0*dist*dist*dist);
                        }
                        colour += float4(sc.xyz * sc.w * atten, 0.0) * Brightness;
                    }

                    //Shade Sparks
                    if(floorT > sparkT && sparkT > 0.0 || floorT < 0.0){
                        float3 sPos = sparkPos + oset;
                        float h = cylinder(sPos, vDir, vel);
                        
                        if(h<0.0){
                            sparkColour += float3(sc.xyz * sc.w);
                        } else {
                            float dist = h * 0.05 + 0.08;
                            float atten = 1.0 / (1.0 + 100.0 * dist * dist * dist);
                            sparkColour += sc.xyz * sc.w * (atten + clamp(1.0 - h * sparkT * 0.05, 0.0, 1.0));
                        }
                    }
                }

                //Shade Sky
                float fade = sqrt(length((fragCoord.xy / _ScreenParams.xy) - float2(0.7, 0.5)));
                float3 sky = float3(0.01, 0.01, 0.05) * (1.0 - fade);
                float3 final = lerp(sky, colour.xyz, colour.w) + sparkColour * Brightness;
                //return final + float3(rand(float2(fragCoord.x * fragCoord.y, _Time))) * 0.002;
                float test = rand(float2(fragCoord.x * fragCoord.y, _Time.y));
                float3 x = float3(test, test, test);
                return final + x;            
                }

            float3 camera(float2 pos) {
                float2 rd = (pos / _ScreenParams.yy - float2( _ScreenParams.x/ _ScreenParams.y*0.5-0.5, 0.0)) * 2.0 - 1.0;
                float3 rdir = normalize(float3(rd.x*0.5, rd.y*0.5, 1.0));
                return trace(float3(-40.0, 20.0, -150), rdir, pos);
            }

            //MAIN PROGRAM

            struct appdata { //inital input data    
                float4 vertex : POSITION;
            };

            struct v2f { //vertex to fragment
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };
      
            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                UNITY_TRANSFER_FOG(o, o.vertex);
                return o;
            }
      
            fixed4 frag (v2f i) : SV_Target {
                // sample the texture
                float3 temp = pow(camera(i.vertex), 0.4545);
                fixed4 colour = float4(temp.x, temp.y, temp.z, 1.0)
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return colour;
            }

            ENDCG
        }
    }
    FallBack "DIFFUSE"
}