using GestionEsports.Esports.Error.Common;

namespace GestionEsports.Esports.Error;

public abstract record StorageError(string Message) : DomainError(Message) {
    public sealed record FileNotFount(string FilePath)
        : StorageError($"No se encontro el archivo en la ruta: {FilePath}");
    
    public sealed record InvalidFormat(string Details)
        : StorageError($"Error por formato de archivo invalido o incompatible: {Details}");
    
    public sealed record WriteError(string Details)
        : StorageError($"Error al escribir en el almacenamiento: {Details}");
    
    public sealed record ReadError(string Details)
        : StorageError($"Error al leer del almacenamiento {Details}");
}

public static class StorageErrors {
    public static DomainError FileNotFount(string filePath) {
        return new StorageError.FileNotFount(filePath);
    }
    public static DomainError InvalidFormat(string Details) {
        return new StorageError.InvalidFormat(Details);
    }
    public static DomainError WriteErrir(string Details) {
        return new StorageError.WriteError(Details);
    }
    public static DomainError ReadError(string Details) {
        return new StorageError.ReadError(Details);
    }
    
}