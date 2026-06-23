class IzinModel {
  final int leaveId;
  final String startDate;
  final String endDate;
  final int dayCount;
  final String status;
  final String? managerNote;
  final String createdDate;
  final String leaveType;

  IzinModel({
    required this.leaveId,
    required this.startDate,
    required this.endDate,
    required this.dayCount,
    required this.status,
    this.managerNote,
    required this.createdDate,
    required this.leaveType,
  });

  factory IzinModel.fromJson(Map<String, dynamic> json) {
    return IzinModel(
      leaveId:     json['LeaveID'] ?? 0,
      startDate:   json['StartDate'] ?? '',
      endDate:     json['EndDate'] ?? '',
      dayCount:    json['DayCount'] ?? 0,
      status:      json['Status'] ?? '',
      managerNote: json['ManagerNote'],
      createdDate: json['CreatedDate'] ?? '',
      leaveType:   json['LeaveType'] ?? '',
    );
  }
}