using UnityEngine;

[System.Serializable]
public sealed class TargetTransform
{
    public Vector3 position;
    public Vector3 rotationEulerAngles;
    public Vector3 scale;

    public TargetTransform() { }
    public TargetTransform(Vector3 pos, Vector3 rotEurAng, Vector3 sca)
    {
        position = pos;
        rotationEulerAngles = rotEurAng;
        scale = sca;
    }
}

public class CameraTarget : MonoBehaviour
{
    #region Singleton
    private static CameraTarget singleton;
    public static CameraTarget Singleton
    {
        get
        {
            if (singleton == null)
            {
                singleton = GameObject.FindGameObjectWithTag("Camera Target").GetComponent<CameraTarget>();
            }
            return singleton;
        }
    }
    #endregion

    #region Fields
    [SerializeField] private GameObject playerCar;
    [SerializeField] private GameObject lastTile;
    [SerializeField] private TargetTransform mainTargetTransformToPlayer;
    [SerializeField] private TargetTransform targetTransformToLastTile;
    #endregion

    #region Properties
    private GamePhase Phase => GameManager.Singleton.Phase;

    private TargetTransform ThisTargetTransform
    {
        get
        {
            if (Phase == GamePhase.Intro)
            {
                return new TargetTransform(new Vector3(-17, 20, 32), new Vector3(33, 150, 0), Vector3.one);
            }

            if (Phase == GamePhase.MainMenu)
            {
                return new TargetTransform(new Vector3(-17, 20, 32), new Vector3(33, 150, 0), Vector3.one);
            }

            if (GameManager.Singleton.IsInGame)
            {
                return new TargetTransform(playerCar.transform.position + mainTargetTransformToPlayer.position, new Vector3(8.5F, 0, 0), Vector3.one);
            }

            if (Phase == GamePhase.WonGame)
            {
                return new TargetTransform(lastTile.transform.position + targetTransformToLastTile.position, new Vector3(8.5F, 0, 0), Vector3.one);
            }

            return new TargetTransform();
        }
    }
    #endregion

    #region Methods
    public void SetLastTile(GameObject tile) => lastTile = tile;
    #endregion

    private void FixedUpdate()
    {
        transform.position = ThisTargetTransform.position;
        transform.rotation = Quaternion.Euler(ThisTargetTransform.rotationEulerAngles);
    }
}