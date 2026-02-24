using MRCD.Application.Abstracts.Handlers;
using MRCD.Application.Alert.Common;
using MRCD.Application.Alert.DTOs;

namespace MRCD.Application.Alert.GetAlertCount;

public sealed record GetAlertCountQuery(
    AlertType Alert
) : IQuery<AlertDTO>;