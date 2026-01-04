using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Actor : MonoBehaviourPun
{
    [SerializeField] private ActorLock _view;
    [SerializeField] private ActorShadow _shadow;
    [SerializeField] private Transform _shadowTransform;
    [SerializeField] Transform[] gizmoPos;

    public Transform ShadowTransform => _shadowTransform;

    private NavMeshPath _movingPath;
    private float _pathDistance;
    private float[] _pathDistances;
    private float _currentMovement;
    private float _acceleration;
    private double _movingDelay;

    public bool IsJumping => _view.IsJumping;

    private readonly int MovementSpeed = 3;
    private readonly int AccelerationSpeed = 5;

    private void Start()
    {
        ActorView.Instance?.RegisterActor(this);

        _movingPath = new NavMeshPath();
    }

    private void Update()
    {
        if (_movingPath.corners.Length <= 1) return;

        // 加速
        if (_currentMovement < _pathDistance)
        {
            _acceleration += Time.deltaTime * AccelerationSpeed;
            _acceleration = Mathf.Clamp(_acceleration, 0, MovementSpeed);

            _currentMovement += _acceleration * Time.deltaTime;
        }
        else
        {
            _currentMovement = _pathDistance;
            _acceleration = 0;

            _shadow.StopMove();
        }

        // 補正
        float deray = 0;
        float acceleration = _acceleration;
        if (!photonView.IsMine)
        {
            for (int i = 0; i < (_movingDelay - 0.001f * (PhotonNetwork.GetPing() / 2)) * 60; i++)
            {
                acceleration += Time.deltaTime * AccelerationSpeed;
                acceleration = Mathf.Clamp(acceleration, 0, MovementSpeed);

                deray += 0.016666f * acceleration;
            }
        }

        // 地点計算
        float count = 0;
        int currentIndex = 0;
        float currentRate = 0;
        for (int i = 0; i < _pathDistances.Length; i++)
        {
            float movemnt = Mathf.Clamp(_currentMovement + deray, 0, _pathDistance);

            float next = count + _pathDistances[i];

            if (movemnt < next)
            {
                currentIndex = i;
                currentRate = (_currentMovement + deray - count) / (next - count);

                break;
            }
            else
            {
                currentIndex = i - 1;
                currentRate = 1;
            }

            count += _pathDistances[i];
        }

        // 座標確定
        var pos = Vector3.Lerp(_movingPath.corners[currentIndex], _movingPath.corners[currentIndex + 1], currentRate);
        pos.y = _shadowTransform.position.y;
        NavMesh.SamplePosition(pos, out NavMeshHit hit, 1.5f, NavMesh.AllAreas);
        _shadowTransform.position = hit.position;

        var def = _movingPath.corners[currentIndex + 1] - _movingPath.corners[currentIndex];
        def.y = 0;
        _shadowTransform.rotation = Quaternion.LookRotation(def);
    }

    [PunRPC]
    public void RPC_ReceivelPositions(Vector3 endPos, Quaternion rotation)
    {
        Vector3 startPos = _shadowTransform.position;

        _shadowTransform.position = endPos;
        _shadowTransform.rotation = rotation;

        CulcPath(startPos, endPos);
    }

    /// <summary>移動開始</summary>
    public IEnumerator SetMovingPosition(Vector3 endPos)
    {
        if (IsJumping) yield break;

        if (photonView.IsMine)
        {
            photonView.RPC("RPC_SetTargetPosition", RpcTarget.Others, _shadow.transform.position, endPos, PhotonNetwork.Time);
        }

        yield return new WaitForSeconds(0.001f * (PhotonNetwork.GetPing() / 2));

        RPC_SetTargetPosition(_shadow.transform.position, endPos, PhotonNetwork.Time);
    }

    /// <summary>移動開始</summary>
    [PunRPC]
    public void RPC_SetTargetPosition(Vector3 startPos, Vector3 position, double sentTime)
    {
        var result = CulcPath(startPos, position);
        if (!result) return;

        _movingDelay = PhotonNetwork.Time - sentTime;
        _shadow.transform.position = _movingPath.corners[0];

        // 回転による減速
        float deg = Quaternion.Angle(_shadow.transform.rotation, Quaternion.LookRotation(_movingPath.corners[1] - _movingPath.corners[0]));
        _acceleration = (1.0f - deg / 180f) * MovementSpeed;

        _currentMovement = 0;

        // アニメ開始
        _shadow.StartMove();
    }

    private bool CulcPath(Vector3 startPos, Vector3 endPos)
    {
        NavMeshPath path = new NavMeshPath();
        if (!NavMesh.CalculatePath(startPos, endPos, NavMesh.AllAreas, path)) return false;

        _movingPath = path;
        _pathDistance = 0;
        _pathDistances = new float[_movingPath.corners.Length];

        for (int i = 0; i < _movingPath.corners.Length - 1; i++)
        {
            var length = (_movingPath.corners[i] - _movingPath.corners[i + 1]).magnitude;

            _pathDistance += length;
            _pathDistances[i] = length;

            gizmoPos[i].position = _movingPath.corners[i];
        }

        return true;
    }
}
