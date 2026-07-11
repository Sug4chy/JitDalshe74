using JitDalshe.Api.Admin.Controllers.AdminUsers.Requests;
using JitDalshe.Api.Admin.Requests;
using JitDalshe.Api.Attributes;
using JitDalshe.Api.Controllers.Base;
using JitDalshe.Api.Models;
using JitDalshe.Application.Admin.Dto;
using JitDalshe.Application.Admin.UseCases.AdminUsers.CreateAdminUser;
using JitDalshe.Application.Admin.UseCases.AdminUsers.DeleteAdminUser;
using JitDalshe.Application.Admin.UseCases.AdminUsers.ListAdminUsers;
using JitDalshe.Application.Admin.UseCases.AdminUsers.ToggleAdminUserStatus;
using JitDalshe.Application.Models;
using JitDalshe.Domain.Common;
using JitDalshe.Domain.Entities.Users;
using JitDalshe.Domain.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JitDalshe.Api.Admin.Controllers.AdminUsers;


[ApiController]
[Route("/api-admin/v1/[controller]")]
[Authorize(Roles = nameof(UserRole.SuperAdmin))]
public sealed class AdminUsersController : AbstractController
{
    /// <summary>
    /// Получение списка всех администраторов с пагинацией и фильтрацией
    /// </summary>
    [HttpGet]
    [ValidateRequest]
    [ProducesResponseType(typeof(PagedResult<AdminUserDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ListAdminUsers(
        [FromQuery] ListWithPaginationRequest paginationRequest,
        [FromQuery] bool? isActive,
        [FromServices] IListAdminUsersUseCase listUseCase,
        CancellationToken ct = default)
    {
        var result = await listUseCase.ListAsync(
            paginationRequest.PageNumber, 
            paginationRequest.PageSize, 
            isActive, 
            ct);

        return result.IsSuccess ? Ok(result.Value) : Error(result.Error);
    }

    /// <summary>
    /// Создание нового администратора
    /// </summary>
    [HttpPost]
    [ValidateRequest]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAdminUser(
        [FromBody] CreateAdminRequest request,
        [FromServices] ICreateAdminUserUseCase createUseCase,
        CancellationToken ct = default)
    {
        var result = await createUseCase.CreateAsync(request.Email, request.Password, ct);
        return result.IsSuccess ? Created() : Error(result.Error);
    }
    
    /// <summary>
    /// Активация или деактивация администратора
    /// </summary>
    [HttpPatch("{id:guid}/active")]
    [ValidateRequest]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiError), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ChangeAdminUserActiveStatus(
        [FromRoute] Guid id,
        [FromBody] ChangeAdminStatusRequest request,
        [FromServices] IToggleAdminUserStatusUseCase changeStatusUseCase,
        CancellationToken ct = default)
    {
        var result = await changeStatusUseCase.ChangeStatusAsync(IdOf<AdminUser>.From(id), request.IsActive, ct);
        return result.IsSuccess ? Ok() : Error(result.Error);
    }
    
    /// <summary>
    /// Удаление администратора
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteAdminUser(
        [FromRoute] Guid id,
        [FromServices] IDeleteAdminUserUseCase deleteUseCase,
        CancellationToken ct = default)
    {
        var result = await deleteUseCase.DeleteAsync(IdOf<AdminUser>.From(id), ct);
        return result.IsSuccess ? NoContent() : Error(result.Error);
    }
}