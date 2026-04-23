using FluentValidation;
using JitDalshe.Ui.Admin.Models;

namespace JitDalshe.Ui.Admin.Api.Consultations.Requests;

public record ChangeConsultationRequestStatusRequest(
    ConsultationRequestStatus Status
);