using UnityEngine;

[CreateAssetMenu(menuName = "Configurations/Create ViewScrollerConfiguration", fileName = "ViewScrollerConfiguration", order = 0)]
public class ViewScrollerConfiguration : ScriptableObject
{
    [SerializeField] private int panelHeight;
    [SerializeField] private int maxY;
    [SerializeField] private int minY;

    public int PanelHeight => panelHeight;

    public int MAXY => maxY;

    public int MINY => minY;
}