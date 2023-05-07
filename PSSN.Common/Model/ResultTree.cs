namespace PSSN.Common.Model;

public class ResultTree
{
    public List<KeyValuePair<TreeId, List<KeyValuePair<TreeId, Dictionary<int, double>>>>> Map { get; set; } = new();
}

public class TreeId : IEquatable<TreeId>
{
    public int Id { get; set; }
    public string Name { get; set; }

    public bool Equals(TreeId? other)
    {
        if(Object.ReferenceEquals(this,other)) return true;
        if(other is null) return false;
        return Id == other.Id;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as TreeId);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id,Name);
    }
}