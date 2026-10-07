using UnityEngine;
using DG.Tweening;

// UI 버튼 위치에서 파티클을 터뜨리는 연출
// Canvas 를 Screen Space - Camera 로 두고, 파티클 렌더러의 Order in Layer 를 캔버스보다 높게 설정해서 UI 위에 그려지게 함
public class UIParticleBurst : MonoBehaviour
{
    [SerializeField] private ParticleSystem burstParticle;
    [SerializeField] private RectTransform target;       // 파티클이 터질 위치 (버튼)
    [SerializeField] private int burstCount = 40;
    [SerializeField] private float punchScale = 0.2f;
    [SerializeField] private float zOffset = -1f;         // 캔버스 평면보다 카메라 쪽으로 살짝 띄움

    public void Play()
    {
        // Screen Space - Camera 캔버스의 UI 는 실제 월드 좌표를 가지므로 그대로 파티클 위치로 사용 가능
        Vector3 pos = target.position;
        pos.z += zOffset;
        burstParticle.transform.position = pos;
        burstParticle.Emit(burstCount);

        target.DOKill(true);
        target.DOPunchScale(Vector3.one * punchScale, 0.35f, 8, 0.8f);
    }
}
