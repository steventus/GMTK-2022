using UnityEngine;
using DG.Tweening;
public class CameraFollow : MonoBehaviour
{
    public float followOffset;
    public float maxFollow;

    public Vector3 _offset;

    private Vector3 oriPos;
    private PlayerController player;
    void Start()
    {
        oriPos = transform.position;
        player = FindObjectOfType<PlayerController>();
        DOTween.Init(false, false);
    }

    void Update()
    {
        Vector3 _playerPos = player.transform.position;

        _offset = (_playerPos - Vector3.zero) * followOffset;
        if (_offset.magnitude > maxFollow)
            _offset = _offset.normalized * maxFollow;

        transform.position = Vector3.MoveTowards(oriPos, _offset, 2);
    }
}
