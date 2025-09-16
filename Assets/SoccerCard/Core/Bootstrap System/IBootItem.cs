using Cysharp.Threading.Tasks;
using System.Threading;

public interface IBootItem
{
    string DisplayName { get; }
    bool RequiresGameObjectInstance { get; }
    UniTask Boot(CancellationToken ct);
}