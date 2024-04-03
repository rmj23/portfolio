public interface IStorageService
    {
         Task<bool> FileExistsAsync(string fileName);
    }