namespace Vehiculos.Cache;

public class CacheLru<TKey, TValue> : ICached<TKey, TValue>
    where TKey : notnull {
    private readonly int _capacity = 3;
    private readonly Dictionary<TKey, TValue> _dataVehiculo = new();
    private readonly LinkedList<TKey> _set = new();

    public void Add(TKey key, TValue value) {
        if (_dataVehiculo.TryGetValue(key, out var existingValue)) {
            _dataVehiculo[key] = value;
            RefreshUsage(key);
            return;
        }
        if (_dataVehiculo.Count >= _capacity) {
            var oldestKey = _set.First!.Value;
            _set.RemoveFirst();
            _dataVehiculo.Remove(oldestKey);
        }
        _dataVehiculo.Add(key, value);
        _set.AddLast(key);
    }
    public TValue? Get(TKey key) {
        if (!_dataVehiculo.TryGetValue(key, out var value)) {
            return default;
        }
        RefreshUsage(key);
        return value;
    }
    public bool Remove(TKey key) {
        if (!_dataVehiculo.Remove(key)) {
            return false;
        }
        _set.Remove(key);
        return true;
    } 
    private void RefreshUsage(TKey key) {
        _set.Remove(key);
        _set.AddLast(key);
    }
}