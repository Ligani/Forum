using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logics.Interfaces
{
    public interface IFileService
    {
        Task<bool> Delete(string fileUrl);
        public Task<string> Upload(IFormFile file);
    }
}
