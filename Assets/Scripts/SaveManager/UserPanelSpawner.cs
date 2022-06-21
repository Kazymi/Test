using UnityEngine;

public class UserPanelSpawner : IUserPanelSpawnerService
{
    private readonly Transform _contentTransform;
    private readonly Transform _disableObjectTransform;
    private readonly LoadPanelItem _userDataPanelPrefab;

    private IPool<LoadPanelItem> _pool;

    public UserPanelSpawner(Transform contentTransform, Transform disableObjectTransform,
        LoadPanelItem userDataPanelPrefab)
    {
        _contentTransform = contentTransform;
        _disableObjectTransform = disableObjectTransform;
        _userDataPanelPrefab = userDataPanelPrefab;
        PoolInitialize();
    }

    private void PoolInitialize()
    {
        const int AmountPanelInPool = 4;
        var factory = new FactoryMonoObject<LoadPanelItem>(_userDataPanelPrefab.gameObject, (_disableObjectTransform));
        _pool = new Pool<LoadPanelItem>(factory, AmountPanelInPool);
    }

    public LoadPanelItem SpawnNewPanelItem()
    {
        var newPanel = _pool.Pull();
        newPanel.transform.parent = _contentTransform;
        return newPanel;
    }
}