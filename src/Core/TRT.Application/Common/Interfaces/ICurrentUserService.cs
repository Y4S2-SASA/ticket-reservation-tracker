/*
 * File: ICurrentUserService.cs
 * Purpose: Handle UserId
 * Author: Dunusinghe A.V/IT20025526
*/
namespace TRT.Application.Common.Interfaces
{
    public interface ICurrentUserService
    {
        string? UserId { get; }
    }
}
