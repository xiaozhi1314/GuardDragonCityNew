public class TableMasterData : CTableData
{
    // 怪物数制表ID
    public int ID;
    // 描述
    public string Desc;
    // 血量
    public float HP;
    // 最大血量
    public float MaxHP;
    // 攻击力
    public float Atk;
    // 攻击冷却时间
    public float AtkCD;
    // 攻击距离
    public float AtkDis;
    // 查找距离
    public float FindDis;
    // 查找冷却时间
    public float FindCD;
    // 速度
    public float Speed;
    // 预设路径
    public string PrefabPath;
    // 对象池中的名字
    public string PoolName;
    // 击杀积分
    public int Score;
    // rvo模型
    public Common.TargetType AgentDefaults;
    // 展示预制体路径
    public string NoticePrefabPath;
}