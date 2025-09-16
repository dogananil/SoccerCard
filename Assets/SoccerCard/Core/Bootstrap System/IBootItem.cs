using Cysharp.Threading.Tasks;
using System.Threading;

public interface IBootItem
{
    UniTask Boot(CancellationToken ct);
    string DisplayName { get; }
}