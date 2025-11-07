# Tài Liệu Database - Fox WMS

Tài liệu này mô tả cấu trúc database cho hệ thống quản lý kho Fox WMS, bao gồm các bảng liên quan đến quy trình sản xuất.

**Phiên bản:** 1.0  
**Ngày cập nhật:** 03/11/2025

---

## Mục Lục

1. [Tổng Quan](#tổng-quan)
2. [Công Đoạn Thổi (Blowing Process)](#công-đoạn-thổi-blowing-process)
3. [Công Đoạn In (Printing Process)](#công-đoạn-in-printing-process)
4. [Công Đoạn Cắt (Cutting Process)](#công-đoạn-cắt-cutting-process)
5. [Công Đoạn Chia (Slitting Process)](#công-đoạn-chia-slitting-process)
6. [Công Đoạn Tua (Rewinding Process)](#công-đoạn-tua-rewinding-process)
7. [Relationships](#relationships)
8. [Indexes & Constraints](#indexes--constraints)
9. [Data Types](#data-types)

---

## Tổng Quan

Hệ thống database được thiết kế để quản lý các công đoạn sản xuất trong nhà máy sản xuất bao bì nhựa. Mỗi công đoạn có:

- **Header Table**: Lưu thông tin chung về công đoạn (ca sản xuất, trưởng ca, ngày sản xuất)
- **Line Table**: Lưu chi tiết từng sản phẩm trong công đoạn

### Các Công Đoạn Chính

1. **Thổi (Blowing)** - Công đoạn thổi màng nhựa
2. **In (Printing)** - Công đoạn in hình lên màng
3. **Cắt (Cutting)** - Công đoạn cắt thành sản phẩm
4. **Chia (Slitting)** - Công đoạn chia cuộn
5. **Tua (Rewinding)** - Công đoạn tua cuộn

---

## Công Đoạn Thổi (Blowing Process)

### Bảng: `FoxWms_BlowingProcess`

Lưu trữ thông tin header của công đoạn thổi.

| Cột                    | Kiểu dữ liệu  | Null | Mô tả                           | Ghi chú               |
| ---------------------- | ------------- | ---- | ------------------------------- | --------------------- |
| `Id`                   | int           | NO   | ID công đoạn thổi               | Primary Key, Identity |
| `ShiftLeaderId`        | int           | NO   | ID trưởng ca                    | FK -> Employees       |
| `IsDraft`              | bit           | NO   | Có phải bản nháp không          | Default: 0            |
| `ProductionDate`       | datetime      | NO   | Ngày sản xuất                   |                       |
| `ProductionShift`      | nvarchar(50)  | NO   | Ca sản xuất                     |                       |
| `TotalBlowingOutput`   | decimal(18,4) | NO   | Tổng sản lượng thổi (kg)        | Auto calculated       |
| `TotalRewindingOutput` | decimal(18,4) | NO   | Tổng sản lượng tua/chia/tờ (kg) | Auto calculated       |
| `TotalReservedOutput`  | decimal(18,4) | NO   | Tổng sản lượng dự trữ (kg)      | Auto calculated       |
| `TotalBlowingLoss`     | decimal(18,4) | NO   | Tổng DC công đoạn thổi (kg)     | Auto calculated       |
| `CreatorId`            | smallint      | NO   | ID người tạo                    | FK -> Users           |
| `ModifierId`           | smallint      | YES  | ID người sửa                    | FK -> Users           |
| `CreatedAt`            | datetime      | NO   | Ngày giờ tạo                    | Default: GETDATE()    |
| `ModifiedAt`           | datetime      | YES  | Ngày giờ sửa                    |                       |
| `RowVersion`           | rowversion    | NO   | Version cho concurrency control | Timestamp             |
| `listOfWorkersText`    | nvarchar(max) | YES  | Danh sách công nhân             |                       |

### Bảng: `FoxWms_BlowingProcessLine`

Lưu trữ chi tiết từng sản phẩm trong công đoạn thổi.

| Cột                             | Kiểu dữ liệu   | Null | Mô tả                               | Ghi chú                     |
| ------------------------------- | -------------- | ---- | ----------------------------------- | --------------------------- |
| `Id`                            | int            | NO   | ID chi tiết                         | Primary Key, Identity       |
| `BlowingProcessId`              | int            | NO   | ID công đoạn thổi                   | FK -> FoxWms_BlowingProcess |
| `ProductionOrderId`             | int            | NO   | ID lệnh sản xuất SAP                |                             |
| `ItemCode`                      | nvarchar(50)   | NO   | Mã hàng                             |                             |
| `ProductionBatch`               | nvarchar(100)  | YES  | Lô sản xuất                         |                             |
| `CardCode`                      | nvarchar(15)   | YES  | Mã khách hàng                       | FK -> BusinessPartners      |
| `ProductType`                   | nvarchar(100)  | YES  | Chủng loại                          |                             |
| `Thickness`                     | nvarchar(50)   | YES  | Độ dày / 1 lá                       |                             |
| `SemiProductWidth`              | nvarchar(50)   | YES  | Khổ màng BTP                        |                             |
| `BlowingMachine`                | nvarchar(50)   | YES  | Máy thổi                            |                             |
| `WorkerId`                      | int            | YES  | ID công nhân thổi                   | FK -> Employees             |
| `BlowingSpeed`                  | nvarchar(50)   | YES  | Tốc độ thổi (kg/giờ)                |                             |
| `StartTime`                     | datetime       | YES  | Thời gian bắt đầu thổi              |                             |
| `EndTime`                       | datetime       | YES  | Thời gian kết thúc thổi             |                             |
| `StopDurationMinutes`           | int            | NO   | Thời gian dừng máy (phút)           | Default: 0                  |
| `StopReason`                    | nvarchar(500)  | YES  | Nguyên nhân dừng máy                |                             |
| `QuantityRolls`                 | decimal(18,4)  | NO   | Sản lượng thổi (số cuộn)            |                             |
| `QuantityKg`                    | decimal(18,4)  | NO   | Sản lượng thổi (số kg)              |                             |
| `RewindOrSplitWeight`           | decimal(18,4)  | NO   | Sản lượng tua/chia/tờ (kg)          |                             |
| `ReservedWeight`                | decimal(18,4)  | NO   | Sản lượng dự trữ (kg)               |                             |
| `RequiredDate`                  | nvarchar(50)   | YES  | Ngày cần hàng                       |                             |
| `IsCompleted`                   | bit            | NO   | Xác nhận hoàn thành                 | Default: 0                  |
| `ActualCompletionDate`          | datetime       | YES  | Ngày hoàn thành thực tế             |                             |
| `DelayReason`                   | nvarchar(500)  | YES  | Nguyên nhân chậm tiến độ (QLSX ghi) |                             |
| `WidthChange`                   | decimal(18,4)  | NO   | Đổi khổ (kg)                        |                             |
| `InnerCoating`                  | decimal(18,4)  | NO   | Tráng lòng (kg)                     |                             |
| `TrimmedEdge`                   | decimal(18,4)  | NO   | Cắt via (kg)                        |                             |
| `ElectricalIssue`               | decimal(18,4)  | NO   | Sự cố điện (kg)                     |                             |
| `MaterialLossKg`                | decimal(18,4)  | NO   | DC nguyên liệu (kg)                 |                             |
| `MaterialLossReason`            | nvarchar(500)  | YES  | Ghi rõ nguyên nhân DC nguyên liệu   |                             |
| `HumanErrorKg`                  | decimal(18,4)  | NO   | DC con người (kg)                   |                             |
| `HumanErrorReason`              | nvarchar(500)  | YES  | Ghi rõ nguyên nhân DC con người     |                             |
| `MachineErrorKg`                | decimal(18,4)  | NO   | DC lỗi máy (kg)                     |                             |
| `MachineErrorReason`            | nvarchar(500)  | YES  | Ghi rõ nguyên nhân DC lỗi máy       |                             |
| `OtherErrorKg`                  | decimal(18,4)  | NO   | DC lỗi khác (kg)                    |                             |
| `OtherErrorReason`              | nvarchar(500)  | YES  | Ghi rõ nguyên nhân DC lỗi khác      |                             |
| `TotalLoss`                     | decimal(18,4)  | NO   | Tổng DC (kg)                        | Auto calculated             |
| `ExcessPO`                      | decimal(18,4)  | NO   | Thừa PO (kg)                        |                             |
| `SemiProductWarehouseConfirmed` | bit            | NO   | Xác nhận của kho BTP                | Default: 0                  |
| `Note`                          | nvarchar(1000) | YES  | Ghi chú                             |                             |
| `BlowingStageInventory`         | decimal(18,4)  | NO   | Tồn kho công đoạn Thổi (kg)         |                             |

---

## Công Đoạn In (Printing Process)

### Bảng: `FoxWms_PrintingProcesses`

Lưu trữ thông tin header của công đoạn in.

| Cột                      | Kiểu dữ liệu  | Null | Mô tả                       | Ghi chú               |
| ------------------------ | ------------- | ---- | --------------------------- | --------------------- |
| `Id`                     | int           | NO   | ID công đoạn in             | Primary Key, Identity |
| `ShiftLeaderId`          | int           | NO   | ID trưởng ca                | FK -> Employees       |
| `IsDraft`                | bit           | NO   | Có phải bản nháp không      | Default: 0            |
| `ProductionDate`         | datetime      | NO   | Ngày sản xuất               |                       |
| `ProductionShift`        | nvarchar(50)  | NO   | Ca sản xuất                 |                       |
| `TotalPrintingOutput`    | decimal(18,4) | NO   | Tổng sản lượng in (kg)      | Auto calculated       |
| `TotalProcessingMold`    | decimal(18,4) | NO   | Tổng DC gia công (kg)       | Auto calculated       |
| `TotalBlowingStageMold`  | decimal(18,4) | NO   | Tổng DC công đoạn Thổi (kg) | Auto calculated       |
| `TotalPrintingStageMold` | decimal(18,4) | NO   | Tổng DC công đoạn In (kg)   | Auto calculated       |
| `CreatorId`              | smallint      | NO   | ID người tạo                | FK -> Users           |
| `ModifierId`             | smallint      | YES  | ID người sửa                | FK -> Users           |
| `CreatedAt`              | datetime      | NO   | Ngày giờ tạo                | Default: GETDATE()    |
| `ModifiedAt`             | datetime      | YES  | Ngày giờ sửa                |                       |
| `RowVersion`             | rowversion    | NO   | Version                     | Timestamp             |

### Bảng: `FoxWms_PrintingProcessLines`

Lưu trữ chi tiết từng sản phẩm trong công đoạn in.

| Cột                        | Kiểu dữ liệu  | Null | Mô tả                              | Ghi chú                        |
| -------------------------- | ------------- | ---- | ---------------------------------- | ------------------------------ |
| `Id`                       | int           | NO   | ID chi tiết                        | Primary Key, Identity          |
| `PrintingProcessId`        | int           | NO   | ID công đoạn in                    | FK -> FoxWms_PrintingProcesses |
| `ProductionOrderId`        | int           | NO   | ID lệnh sản xuất SAP               |                                |
| `ItemCode`                 | nvarchar(50)  | NO   | Mã hàng                            |                                |
| `ProductionBatch`          | nvarchar(100) | YES  | Lô sản xuất                        |                                |
| `CardCode`                 | nvarchar(15)  | YES  | Mã khách hàng                      | FK -> BusinessPartners         |
| `ProductType`              | nvarchar(100) | YES  | Chủng loại                         |                                |
| `Thickness`                | nvarchar(50)  | YES  | Độ dày / 1 lá                      |                                |
| `SemiProductWidth`         | nvarchar(50)  | YES  | Khổ màng BTP                       |                                |
| `PrintPatternName`         | nvarchar(200) | YES  | Tên hình in                        |                                |
| `ColorCount`               | nvarchar(50)  | YES  | Số màu in                          |                                |
| `PrintingMachine`          | nvarchar(50)  | YES  | Máy in                             |                                |
| `WorkerId`                 | int           | YES  | ID công nhân in                    | FK -> Employees                |
| `PrintingSpeed`            | nvarchar(50)  | YES  | Tốc độ in                          |                                |
| `StartTime`                | datetime      | YES  | Thời gian bắt đầu in               |                                |
| `EndTime`                  | datetime      | YES  | Thời gian kết thúc in              |                                |
| `MachineStopMinutes`       | decimal(18,4) | NO   | Thời gian dừng máy (phút)          |                                |
| `StopReason`               | nvarchar(500) | YES  | Nguyên nhân dừng máy               |                                |
| `RollCount`                | int           | NO   | Số cuộn                            |                                |
| `PieceCount`               | int           | NO   | Số chiếc                           |                                |
| `QuantityKg`               | decimal(18,4) | YES  | Số kg                              |                                |
| `RequiredDate`             | datetime      | YES  | Ngày cần hàng                      |                                |
| `IsCompleted`              | bit           | NO   | Xác nhận hoàn thành                | Default: 0                     |
| `ActualCompletionDate`     | datetime      | YES  | Ngày hoàn thành thực tế            |                                |
| `DelayReason`              | nvarchar(500) | YES  | Nguyên nhân chậm tiến độ           |                                |
| `ProcessingLossKg`         | decimal(18,4) | NO   | DC gia công (kg)                   |                                |
| `ProcessingLossReason`     | nvarchar(500) | YES  | DC gia công - nguyên nhân          |                                |
| `BlowingLossKg`            | decimal(18,4) | NO   | DC do công đoạn thổi (kg)          |                                |
| `BlowingLossReason`        | nvarchar(500) | YES  | DC do công đoạn thổi - nguyên nhân |                                |
| `OppRollHeadKg`            | decimal(18,4) | NO   | Đầu cuộn OPP (kg)                  |                                |
| `OppRollHeadReason`        | nvarchar(500) | YES  | Đầu cuộn OPP - nguyên nhân         |                                |
| `HumanLossKg`              | decimal(18,4) | NO   | Con người (kg)                     |                                |
| `HumanLossReason`          | nvarchar(500) | YES  | Con người - nguyên nhân            |                                |
| `MachineLossKg`            | decimal(18,4) | NO   | Lỗi máy (kg)                       |                                |
| `MachineLossReason`        | nvarchar(500) | YES  | Lỗi máy - nguyên nhân              |                                |
| `TotalLossKg`              | decimal(18,4) | NO   | Tổng DC (kg)                       | Auto calculated                |
| `ExcessPO`                 | decimal(18,4) | NO   | Thừa PO                            | Default: 0                     |
| `BtpWarehouseConfirmation` | bit           | NO   | Xác nhận của kho BTP               | Default: 0                     |
| `PrintingStageInventoryKg` | decimal(18,4) | NO   | Tồn kho ở công đoạn In (kg)        |                                |

---

## Công Đoạn Cắt (Cutting Process)

### Bảng: `FoxWms_CuttingProcesses`

Lưu trữ thông tin header của công đoạn cắt.

| Cột                      | Kiểu dữ liệu  | Null | Mô tả                       | Ghi chú               |
| ------------------------ | ------------- | ---- | --------------------------- | --------------------- |
| `Id`                     | int           | NO   | ID công đoạn cắt            | Primary Key, Identity |
| `ShiftLeaderId`          | int           | NO   | ID trưởng ca                | FK -> Employees       |
| `IsDraft`                | bit           | NO   | Có phải bản nháp không      | Default: 0            |
| `ProductionDate`         | datetime      | NO   | Ngày sản xuất               |                       |
| `ProductionShift`        | nvarchar(50)  | NO   | Ca sản xuất                 |                       |
| `TotalCuttingOutput`     | decimal(18,4) | NO   | Tổng sản lượng cắt (kg)     | Auto calculated       |
| `TotalFoldedCount`       | decimal(18,4) | NO   | Tổng số lượng gấp xúc       | Auto calculated       |
| `TotalProcessingMold`    | decimal(18,4) | NO   | Tổng DC gia công (kg)       | Auto calculated       |
| `TotalBlowingStageMold`  | decimal(18,4) | NO   | Tổng DC công đoạn Thổi (kg) | Auto calculated       |
| `TotalPrintingStageMold` | decimal(18,4) | NO   | Tổng DC công đoạn In (kg)   | Auto calculated       |
| `TotalCuttingStageMold`  | decimal(18,4) | NO   | Tổng DC công đoạn Cắt (kg)  | Auto calculated       |
| `CreatorId`              | smallint      | NO   | ID người tạo                | FK -> Users           |
| `ModifierId`             | smallint      | YES  | ID người sửa                | FK -> Users           |
| `CreatedAt`              | datetime      | NO   | Ngày giờ tạo                | Default: GETDATE()    |
| `ModifiedAt`             | datetime      | YES  | Ngày giờ sửa                |                       |

### Bảng: `FoxWms_CuttingProcessesLine`

Lưu trữ chi tiết từng sản phẩm trong công đoạn cắt.

| Cột                     | Kiểu dữ liệu  | Null | Mô tả                              | Ghi chú                       |
| ----------------------- | ------------- | ---- | ---------------------------------- | ----------------------------- |
| `Id`                    | int           | NO   | ID chi tiết                        | Primary Key, Identity         |
| `CuttingProcessId`      | int           | NO   | ID công đoạn cắt                   | FK -> FoxWms_CuttingProcesses |
| `ProductionOrderId`     | int           | NO   | ID lệnh sản xuất SAP               |                               |
| `ItemCode`              | nvarchar(50)  | NO   | Mã hàng                            |                               |
| `ProductionBatch`       | nvarchar(100) | YES  | Lô sản xuất                        |                               |
| `CardCode`              | nvarchar(15)  | YES  | Mã khách hàng                      | FK -> BusinessPartners        |
| `ProductType`           | nvarchar(100) | YES  | Chủng loại                         |                               |
| `Thickness`             | nvarchar(50)  | YES  | Độ dày / 1 lá                      |                               |
| `SemiProductWidth`      | nvarchar(50)  | YES  | Khổ màng BTP                       |                               |
| `Size`                  | nvarchar(100) | YES  | Kích thước                         |                               |
| `ColorCount`            | nvarchar(50)  | YES  | Số màu in                          |                               |
| `CuttingMachine`        | nvarchar(50)  | YES  | Máy cắt                            |                               |
| `WorkerId`              | int           | YES  | ID công nhân cắt                   | FK -> Employees               |
| `CuttingSpeed`          | decimal(18,4) | NO   | Tốc độ cắt                         |                               |
| `StartTime`             | datetime      | YES  | Thời gian bắt đầu cắt              |                               |
| `EndTime`               | datetime      | YES  | Thời gian kết thúc cắt             |                               |
| `MachineStopMinutes`    | decimal(18,4) | NO   | Thời gian dừng máy (phút)          |                               |
| `StopReason`            | nvarchar(500) | YES  | Nguyên nhân dừng máy               |                               |
| `PieceCount`            | decimal(18,4) | NO   | Số chiếc (sản lượng cắt)           |                               |
| `QuantityKg`            | decimal(18,4) | NO   | Số kg (sản lượng cắt)              |                               |
| `BagCount`              | decimal(18,4) | NO   | Số bao (sản lượng cắt)             |                               |
| `FoldedCount`           | decimal(18,4) | NO   | Số lượng gấp xúc                   |                               |
| `RequiredDate`          | datetime      | YES  | Ngày cần hàng                      |                               |
| `IsCompleted`           | bit           | NO   | Xác nhận hoàn thành                | Default: 0                    |
| `ActualCompletionDate`  | datetime      | YES  | Ngày hoàn thành thực tế            |                               |
| `DelayReason`           | nvarchar(500) | YES  | Nguyên nhân chậm tiến độ           |                               |
| `ProcessingLossKg`      | decimal(18,4) | NO   | DC gia công (kg)                   |                               |
| `ProcessingLossReason`  | nvarchar(500) | YES  | DC gia công - nguyên nhân          |                               |
| `BlowingLossKg`         | decimal(18,4) | NO   | DC do công đoạn thổi (kg)          |                               |
| `BlowingLossReason`     | nvarchar(500) | YES  | DC do công đoạn thổi - nguyên nhân |                               |
| `PrintingLossKg`        | decimal(18,4) | NO   | DC do công đoạn in (kg)            |                               |
| `PrintingLossReason`    | nvarchar(500) | YES  | DC do công đoạn in - nguyên nhân   |                               |
| `PrintingMachine`       | nvarchar(50)  | YES  | Số máy in                          |                               |
| `TransferKg`            | decimal(18,4) | NO   | Chuyển hàng (kg)                   |                               |
| `HumanLossKg`           | decimal(18,4) | NO   | DC do cắt dán - Con người (kg)     |                               |
| `HumanLossReason`       | nvarchar(500) | YES  | DC - Nguyên nhân con người         |                               |
| `MachineLossKg`         | decimal(18,4) | NO   | DC do cắt dán - Lỗi máy (kg)       |                               |
| `MachineLossReason`     | nvarchar(500) | YES  | DC - Nguyên nhân lỗi máy           |                               |
| `TotalLossKg`           | decimal(18,4) | NO   | Tổng DC (kg)                       | Auto calculated               |
| `ExcessPOLess5Kg`       | decimal(18,4) | NO   | Thừa PO - Cuộn dưới 5kg            |                               |
| `ExcessPOOver5Kg`       | decimal(18,4) | NO   | Thừa PO - Cuộn trên 5kg            |                               |
| `ExcessPOCut`           | decimal(18,4) | NO   | Thừa PO - Hàng cắt (kg)            |                               |
| `BtpWarehouseConfirmed` | bit           | YES  | Xác nhận của kho BTP               | Default: NULL                 |
| `RemainingInventoryKg`  | decimal(18,4) | NO   | Tồn (kg)                           |                               |

---

## Công Đoạn Chia (Slitting Process)

### Bảng: `FoxWms_SlittingProcess`

Lưu trữ thông tin header của công đoạn chia.

| Cột                      | Kiểu dữ liệu  | Null | Mô tả                       | Ghi chú               |
| ------------------------ | ------------- | ---- | --------------------------- | --------------------- |
| `Id`                     | int           | NO   | ID công đoạn chia           | Primary Key, Identity |
| `ShiftLeaderId`          | int           | NO   | ID trưởng ca                | FK -> Employees       |
| `IsDraft`                | bit           | NO   | Có phải bản nháp không      | Default: 0            |
| `ProductionDate`         | datetime      | NO   | Ngày sản xuất               |                       |
| `ProductionShift`        | nvarchar(50)  | NO   | Ca sản xuất                 |                       |
| `TotalProcessingMold`    | decimal(18,4) | NO   | Tổng DC gia công (kg)       | Auto calculated       |
| `TotalBlowingStageMold`  | decimal(18,4) | NO   | Tổng DC công đoạn Thổi (kg) | Auto calculated       |
| `TotalPrintingStageMold` | decimal(18,4) | NO   | Tổng DC công đoạn In (kg)   | Auto calculated       |
| `TotalSlittingStageMold` | decimal(18,4) | NO   | Tổng DC công đoạn Chia (kg) | Auto calculated       |
| `CreatorId`              | smallint      | NO   | ID người tạo                | FK -> Users           |
| `ModifierId`             | smallint      | YES  | ID người sửa                | FK -> Users           |
| `CreatedAt`              | datetime      | NO   | Ngày giờ tạo                | Default: GETDATE()    |
| `ModifiedAt`             | datetime      | YES  | Ngày giờ sửa                |                       |
| `RowVersion`             | rowversion    | NO   | Version                     | Timestamp             |

### Bảng: `FoxWms_SlittingProcessLine`

Lưu trữ chi tiết từng sản phẩm trong công đoạn chia.

| Cột                     | Kiểu dữ liệu  | Null | Mô tả                                 | Ghi chú                      |
| ----------------------- | ------------- | ---- | ------------------------------------- | ---------------------------- |
| `Id`                    | int           | NO   | ID chi tiết                           | Primary Key, Identity        |
| `SlittingProcessId`     | int           | NO   | ID công đoạn chia                     | FK -> FoxWms_SlittingProcess |
| `ProductionOrderId`     | int           | NO   | ID lệnh sản xuất SAP                  |                              |
| `ItemCode`              | nvarchar(50)  | NO   | Mã hàng                               |                              |
| `ProductionBatch`       | nvarchar(100) | YES  | Lô sản xuất                           |                              |
| `CardCode`              | nvarchar(15)  | YES  | Mã khách hàng                         | FK -> BusinessPartners       |
| `ProductType`           | nvarchar(100) | YES  | Chủng loại                            |                              |
| `ProductTypeName`       | nvarchar(200) | YES  | Tên chủng loại                        |                              |
| `Thickness`             | nvarchar(50)  | YES  | Độ dày / 1 lá                         |                              |
| `SemiProductWidth`      | nvarchar(50)  | YES  | Khổ màng BTP                          |                              |
| `PrintPatternName`      | nvarchar(200) | YES  | Tên hình in                           |                              |
| `ColorCount`            | nvarchar(50)  | YES  | Số màu in                             |                              |
| `SlittingMachine`       | nvarchar(50)  | NO   | Máy chia                              |                              |
| `WorkerId`              | int           | YES  | ID công nhân chia                     | FK -> Employees              |
| `SlittingSpeed`         | decimal(18,4) | YES  | Tốc độ chia                           |                              |
| `StartTime`             | datetime      | YES  | Thời gian bắt đầu chia                |                              |
| `EndTime`               | datetime      | YES  | Thời gian kết thúc chia               |                              |
| `MachineStopMinutes`    | decimal(18,4) | NO   | Thời gian dừng máy (phút)             |                              |
| `StopReason`            | nvarchar(500) | YES  | Nguyên nhân dừng máy                  |                              |
| `RollCount`             | decimal(18,4) | NO   | Số cuộn                               |                              |
| `PieceCount`            | decimal(18,4) | NO   | Số chiếc                              |                              |
| `QuantityKg`            | decimal(18,4) | NO   | Số kg                                 |                              |
| `BoxCount`              | decimal(18,4) | NO   | Số thùng                              |                              |
| `RequiredDate`          | datetime      | YES  | Ngày cần hàng                         |                              |
| `IsCompleted`           | bit           | NO   | Xác nhận hoàn thành                   | Default: 0                   |
| `ActualCompletionDate`  | datetime      | YES  | Ngày hoàn thành thực tế               |                              |
| `DelayReason`           | nvarchar(500) | YES  | Nguyên nhân chậm tiến độ              |                              |
| `ProcessingLossKg`      | decimal(18,4) | NO   | DC gia công (kg)                      |                              |
| `ProcessingLossReason`  | nvarchar(500) | YES  | DC gia công - nguyên nhân             |                              |
| `BlowingLossKg`         | decimal(18,4) | NO   | DC do công đoạn thổi (kg)             |                              |
| `BlowingLossReason`     | nvarchar(500) | YES  | DC do công đoạn thổi - nguyên nhân    |                              |
| `PrintingLossKg`        | decimal(18,4) | NO   | DC do công đoạn in (kg)               |                              |
| `PrintingLossReason`    | nvarchar(500) | YES  | DC do công đoạn in - nguyên nhân      |                              |
| `PrintingMachine`       | nvarchar(50)  | YES  | Số máy in                             |                              |
| `CutViaKg`              | decimal(18,4) | NO   | Cắt via (kg)                          |                              |
| `HumanLossKg`           | decimal(18,4) | NO   | DC do công đoạn chia - Con người (kg) |                              |
| `HumanLossReason`       | nvarchar(500) | YES  | DC - Nguyên nhân con người            |                              |
| `MachineLossKg`         | decimal(18,4) | NO   | DC do công đoạn chia - Lỗi máy (kg)   |                              |
| `MachineLossReason`     | nvarchar(500) | YES  | DC - Nguyên nhân lỗi máy              |                              |
| `TotalLossKg`           | decimal(18,4) | NO   | Tổng DC (kg)                          | Auto calculated              |
| `ExcessPOPrinting`      | decimal(18,4) | NO   | Thừa PO - BTP In (kg)                 |                              |
| `ExcessPOSlitting`      | decimal(18,4) | NO   | Thừa PO - TP Chia (kg)                |                              |
| `BtpWarehouseConfirmed` | bit           | NO   | Xác nhận của kho BTP                  | Default: 0                   |

---

## Công Đoạn Tua (Rewinding Process)

### Bảng: `FoxWms_RewindingProcess`

Lưu trữ thông tin header của công đoạn tua.

| Cột                       | Kiểu dữ liệu  | Null | Mô tả                       | Ghi chú               |
| ------------------------- | ------------- | ---- | --------------------------- | --------------------- |
| `Id`                      | int           | NO   | ID công đoạn tua            | Primary Key, Identity |
| `ShiftLeaderId`           | int           | NO   | ID trưởng ca                | FK -> Employees       |
| `IsDraft`                 | bit           | NO   | Có phải bản nháp không      | Default: 0            |
| `ProductionDate`          | datetime      | NO   | Ngày sản xuất               |                       |
| `ProductionShift`         | nvarchar(50)  | NO   | Ca sản xuất                 |                       |
| `TotalRewindingOutput`    | decimal(18,4) | NO   | Tổng sản lượng tua (kg)     | Auto calculated       |
| `TotalBlowingStageMold`   | decimal(18,4) | NO   | Tổng DC công đoạn Thổi (kg) | Auto calculated       |
| `TotalRewindingStageMold` | decimal(18,4) | NO   | Tổng DC công đoạn Tua (kg)  | Auto calculated       |

### Bảng: `FoxWms_RewindingProcessLine`

Lưu trữ chi tiết từng sản phẩm trong công đoạn tua.

| Cột                    | Kiểu dữ liệu  | Null | Mô tả                                | Ghi chú                       |
| ---------------------- | ------------- | ---- | ------------------------------------ | ----------------------------- |
| `Id`                   | int           | NO   | ID chi tiết                          | Primary Key, Identity         |
| `RewindingProcessId`   | int           | NO   | ID công đoạn tua                     | FK -> FoxWms_RewindingProcess |
| `ProductionOrderId`    | int           | NO   | ID lệnh sản xuất SAP                 |                               |
| `ItemCode`             | nvarchar(50)  | NO   | Mã hàng                              |                               |
| `ProductionBatch`      | nvarchar(100) | YES  | Lô sản xuất                          |                               |
| `CardCode`             | nvarchar(15)  | YES  | Mã khách hàng                        | FK -> BusinessPartners        |
| `ProductType`          | nvarchar(100) | YES  | Chủng loại                           |                               |
| `Thickness`            | nvarchar(50)  | YES  | Độ dày / 1 lá                        |                               |
| `SemiProductWidth`     | nvarchar(50)  | YES  | Khổ màng BTP                         |                               |
| `RewindingMachine`     | nvarchar(50)  | YES  | Máy tua                              |                               |
| `WorkerId`             | int           | YES  | ID công nhân tua                     | FK -> Employees               |
| `StartTime`            | datetime      | YES  | Thời gian bắt đầu tua                |                               |
| `EndTime`              | datetime      | YES  | Thời gian kết thúc tua               |                               |
| `MachineStopMinutes`   | decimal(18,4) | NO   | Thời gian dừng máy (phút)            |                               |
| `StopReason`           | nvarchar(500) | YES  | Nguyên nhân dừng máy                 |                               |
| `RollCount`            | decimal(18,4) | NO   | Số cuộn                              |                               |
| `QuantityKg`           | decimal(18,4) | NO   | Số kg                                |                               |
| `RequiredDate`         | datetime      | YES  | Ngày cần hàng                        |                               |
| `IsCompleted`          | bit           | NO   | Xác nhận hoàn thành                  | Default: 0                    |
| `ActualCompletionDate` | datetime      | YES  | Ngày hoàn thành thực tế              |                               |
| `DelayReason`          | nvarchar(500) | YES  | Nguyên nhân chậm tiến độ             |                               |
| `BlowingLossKg`        | decimal(18,4) | NO   | DC do công đoạn thổi (kg)            |                               |
| `BlowingLossReason`    | nvarchar(500) | YES  | DC do công đoạn thổi - nguyên nhân   |                               |
| `HumanLossKg`          | decimal(18,4) | NO   | DC do công đoạn tua - Con người (kg) |                               |
| `HumanLossReason`      | nvarchar(500) | YES  | DC - Nguyên nhân con người           |                               |
| `MachineLossKg`        | decimal(18,4) | NO   | DC do công đoạn tua - Lỗi máy (kg)   |                               |
| `MachineLossReason`    | nvarchar(500) | YES  | DC - Nguyên nhân lỗi máy             |                               |
| `TotalLossKg`          | decimal(18,4) | NO   | Tổng DC (kg)                         | Auto calculated               |

---

## Relationships

### Entity Relationship Diagram (ERD)

```
┌─────────────────┐
│    Employees    │
└────────┬────────┘
         │
         │ 1:N (ShiftLeaderId, WorkerId)
         │
    ┌────┴────┬────────┬────────┬────────┬────────┐
    │         │        │        │        │        │
┌───▼──────┐ ┌▼─────┐ ┌▼──────┐ ┌▼──────┐ ┌▼─────┐
│ Blowing  │ │Print │ │Cutting│ │Slitt  │ │Rewnd │
│ Process  │ │Proc  │ │Process│ │Process│ │Proc  │
└───┬──────┘ └┬─────┘ └┬──────┘ └┬──────┘ └┬─────┘
    │ 1:N     │ 1:N    │ 1:N     │ 1:N     │ 1:N
┌───▼──────┐ ┌▼─────┐ ┌▼──────┐ ┌▼──────┐ ┌▼─────┐
│ Blowing  │ │Print │ │Cutting│ │Slitt  │ │Rewnd │
│ Proc Line│ │Line  │ │Line   │ │Line   │ │Line  │
└──────────┘ └──────┘ └───────┘ └───────┘ └──────┘
     │          │         │         │         │
     └──────────┴─────────┴─────────┴─────────┘
                    │ N:1
          ┌─────────▼─────────┐
          │ BusinessPartners  │
          └───────────────────┘
```

### Foreign Key Constraints

| Bảng                   | Cột                  | References                 | On Delete |
| ---------------------- | -------------------- | -------------------------- | --------- |
| All Process Tables     | `ShiftLeaderId`      | Employees(Id)              | RESTRICT  |
| All Process Tables     | `CreatorId`          | Users(Id)                  | RESTRICT  |
| All Process Tables     | `ModifierId`         | Users(Id)                  | SET NULL  |
| All ProcessLine Tables | `WorkerId`           | Employees(Id)              | SET NULL  |
| All ProcessLine Tables | `CardCode`           | BusinessPartners(CardCode) | SET NULL  |
| BlowingProcessLine     | `BlowingProcessId`   | BlowingProcess(Id)         | CASCADE   |
| PrintingProcessLine    | `PrintingProcessId`  | PrintingProcess(Id)        | CASCADE   |
| CuttingProcessLine     | `CuttingProcessId`   | CuttingProcess(Id)         | CASCADE   |
| SlittingProcessLine    | `SlittingProcessId`  | SlittingProcess(Id)        | CASCADE   |
| RewindingProcessLine   | `RewindingProcessId` | RewindingProcess(Id)       | CASCADE   |

---

## Indexes & Constraints

### Recommended Indexes

```sql
-- Indexes cho Performance
CREATE INDEX IX_BlowingProcess_ProductionDate ON FoxWms_BlowingProcess(ProductionDate DESC);
CREATE INDEX IX_BlowingProcess_ShiftLeaderId ON FoxWms_BlowingProcess(ShiftLeaderId);
CREATE INDEX IX_BlowingProcessLine_BlowingProcessId ON FoxWms_BlowingProcessLine(BlowingProcessId);
CREATE INDEX IX_BlowingProcessLine_ProductionOrderId ON FoxWms_BlowingProcessLine(ProductionOrderId);

CREATE INDEX IX_PrintingProcess_ProductionDate ON FoxWms_PrintingProcesses(ProductionDate DESC);
CREATE INDEX IX_PrintingProcess_ShiftLeaderId ON FoxWms_PrintingProcesses(ShiftLeaderId);
CREATE INDEX IX_PrintingProcessLine_PrintingProcessId ON FoxWms_PrintingProcessLines(PrintingProcessId);
CREATE INDEX IX_PrintingProcessLine_ProductionOrderId ON FoxWms_PrintingProcessLines(ProductionOrderId);

CREATE INDEX IX_CuttingProcess_ProductionDate ON FoxWms_CuttingProcesses(ProductionDate DESC);
CREATE INDEX IX_CuttingProcess_ShiftLeaderId ON FoxWms_CuttingProcesses(ShiftLeaderId);
CREATE INDEX IX_CuttingProcessLine_CuttingProcessId ON FoxWms_CuttingProcessesLine(CuttingProcessId);
CREATE INDEX IX_CuttingProcessLine_ProductionOrderId ON FoxWms_CuttingProcessesLine(ProductionOrderId);

CREATE INDEX IX_SlittingProcess_ProductionDate ON FoxWms_SlittingProcess(ProductionDate DESC);
CREATE INDEX IX_SlittingProcess_ShiftLeaderId ON FoxWms_SlittingProcess(ShiftLeaderId);
CREATE INDEX IX_SlittingProcessLine_SlittingProcessId ON FoxWms_SlittingProcessLine(SlittingProcessId);
CREATE INDEX IX_SlittingProcessLine_ProductionOrderId ON FoxWms_SlittingProcessLine(ProductionOrderId);

CREATE INDEX IX_RewindingProcess_ProductionDate ON FoxWms_RewindingProcess(ProductionDate DESC);
CREATE INDEX IX_RewindingProcess_ShiftLeaderId ON FoxWms_RewindingProcess(ShiftLeaderId);
CREATE INDEX IX_RewindingProcessLine_RewindingProcessId ON FoxWms_RewindingProcessLine(RewindingProcessId);
CREATE INDEX IX_RewindingProcessLine_ProductionOrderId ON FoxWms_RewindingProcessLine(ProductionOrderId);
```

### Check Constraints

```sql
-- Ensure totals are non-negative
ALTER TABLE FoxWms_BlowingProcess
  ADD CONSTRAINT CK_BlowingProcess_TotalBlowingOutput
  CHECK (TotalBlowingOutput >= 0);

ALTER TABLE FoxWms_PrintingProcesses
  ADD CONSTRAINT CK_PrintingProcess_TotalPrintingOutput
  CHECK (TotalPrintingOutput >= 0);

ALTER TABLE FoxWms_CuttingProcesses
  ADD CONSTRAINT CK_CuttingProcess_TotalCuttingOutput
  CHECK (TotalCuttingOutput >= 0);

-- Ensure dates are logical
ALTER TABLE FoxWms_BlowingProcessLine
  ADD CONSTRAINT CK_BlowingLine_StartEndTime
  CHECK (EndTime IS NULL OR StartTime IS NULL OR EndTime >= StartTime);

ALTER TABLE FoxWms_PrintingProcessLines
  ADD CONSTRAINT CK_PrintingLine_StartEndTime
  CHECK (EndTime IS NULL OR StartTime IS NULL OR EndTime >= StartTime);

ALTER TABLE FoxWms_CuttingProcessesLine
  ADD CONSTRAINT CK_CuttingLine_StartEndTime
  CHECK (EndTime IS NULL OR StartTime IS NULL OR EndTime >= StartTime);
```

---

## Data Types

### Precision Standards

| Kiểu            | Sử dụng cho                          | Precision |
| --------------- | ------------------------------------ | --------- |
| `decimal(18,4)` | Số lượng, trọng lượng, DC            | 18,4      |
| `int`           | ID, số đếm (piece, roll khi integer) | -         |
| `smallint`      | User IDs                             | -         |
| `bit`           | Boolean flags                        | -         |
| `datetime`      | Dates, timestamps                    | -         |
| `nvarchar`      | Text fields                          | Varies    |
| `rowversion`    | Concurrency control                  | -         |

### Common Field Patterns

| Tên trường          | Kiểu          | Mô tả                  |
| ------------------- | ------------- | ---------------------- |
| `Id`                | int           | Primary key            |
| `*ProcessId`        | int           | Foreign key to header  |
| `ProductionOrderId` | int           | SAP Production Order   |
| `ItemCode`          | nvarchar(50)  | Product code           |
| `CardCode`          | nvarchar(15)  | Customer code          |
| `WorkerId`          | int           | Employee ID            |
| `*LossKg`           | decimal(18,4) | Loss amounts           |
| `*Reason`           | nvarchar(500) | Reason descriptions    |
| `IsCompleted`       | bit           | Completion flag        |
| `IsDraft`           | bit           | Draft flag             |
| `CreatedAt`         | datetime      | Creation timestamp     |
| `ModifiedAt`        | datetime      | Modification timestamp |

---

## Business Rules

### Tính Toán Tự Động (Calculated Fields)

#### Blowing Process

```
TotalLoss = WidthChange + InnerCoating + TrimmedEdge + ElectricalIssue
          + MaterialLossKg + HumanErrorKg + MachineErrorKg + OtherErrorKg

TotalBlowingOutput = SUM(Lines.QuantityKg)
TotalRewindingOutput = SUM(Lines.RewindOrSplitWeight)
TotalReservedOutput = SUM(Lines.ReservedWeight)
TotalBlowingLoss = SUM(Lines.TotalLoss)
```

#### Printing Process

```
TotalLossKg = ProcessingLossKg + BlowingLossKg + OppRollHeadKg
            + HumanLossKg + MachineLossKg

TotalPrintingOutput = SUM(Lines.QuantityKg)
TotalProcessingMold = SUM(Lines.ProcessingLossKg)
TotalBlowingStageMold = SUM(Lines.BlowingLossKg)
TotalPrintingStageMold = SUM(Lines.OppRollHeadKg + HumanLossKg + MachineLossKg)
```

#### Cutting Process

```
TotalLossKg = ProcessingLossKg + BlowingLossKg + PrintingLossKg
            + HumanLossKg + MachineLossKg

TotalCuttingOutput = SUM(Lines.QuantityKg)
TotalFoldedCount = SUM(Lines.FoldedCount)
TotalProcessingMold = SUM(Lines.ProcessingLossKg)
```

#### Slitting Process

```
TotalLossKg = ProcessingLossKg + BlowingLossKg + PrintingLossKg
            + HumanLossKg + MachineLossKg

TotalProcessingMold = SUM(Lines.ProcessingLossKg)
TotalBlowingStageMold = SUM(Lines.BlowingLossKg)
TotalPrintingStageMold = SUM(Lines.PrintingLossKg)
TotalSlittingStageMold = SUM(Lines.HumanLossKg + MachineLossKg)
```

#### Rewinding Process

```
TotalLossKg = BlowingLossKg + HumanLossKg + MachineLossKg

TotalRewindingOutput = SUM(Lines.QuantityKg)
TotalBlowingStageMold = SUM(Lines.BlowingLossKg)
TotalRewindingStageMold = SUM(Lines.HumanLossKg + MachineLossKg)
```

### Quy Tắc Validation

1. **ProductionDate** không được là ngày tương lai
2. **EndTime** phải >= **StartTime** (nếu cả hai không null)
3. Các trường số lượng, trọng lượng phải >= 0
4. **ShiftLeaderId** phải tồn tại trong bảng Employees
5. **WorkerId** (nếu có) phải tồn tại trong bảng Employees
6. **CardCode** (nếu có) phải tồn tại trong bảng BusinessPartners
7. **ProductionOrderId** phải tồn tại trong SAP system

### Draft Mode

- Khi `IsDraft = true`: Cho phép lưu dữ liệu chưa đầy đủ
- Khi `IsDraft = false`: Phải validate đầy đủ các trường required

---

## Thuật Ngữ & Viết Tắt

| Thuật ngữ | Ý nghĩa                                 |
| --------- | --------------------------------------- |
| **DC**    | Defective/Damage - Sản phẩm lỗi/hư hỏng |
| **BTP**   | Bán Thành Phẩm                          |
| **TP**    | Thành Phẩm                              |
| **PO**    | Production Order - Đơn hàng sản xuất    |
| **QLSX**  | Quản Lý Sản Xuất                        |
| **PE**    | Polyethylene - Loại nhựa                |
| **OPP**   | Oriented Polypropylene                  |
| **FK**    | Foreign Key                             |

---

## Migration Notes

### Entity Framework Core Migrations

```bash
# Create new migration
dotnet ef migrations add MigrationName --project Fox.Whs

# Update database
dotnet ef database update --project Fox.Whs

# Remove last migration
dotnet ef migrations remove --project Fox.Whs
```

### Backup Strategy

```sql
-- Full backup
BACKUP DATABASE FoxWms TO DISK = 'C:\Backups\FoxWms_Full.bak'

-- Differential backup
BACKUP DATABASE FoxWms TO DISK = 'C:\Backups\FoxWms_Diff.bak'
  WITH DIFFERENTIAL

-- Transaction log backup
BACKUP LOG FoxWms TO DISK = 'C:\Backups\FoxWms_Log.trn'
```

---

## Changelog

| Version | Date       | Changes                               |
| ------- | ---------- | ------------------------------------- |
| 1.0     | 03/11/2025 | Initial database schema documentation |

---

**Ghi chú:**

- Tài liệu này được tự động sinh từ Models trong ứng dụng
- Cấu trúc thực tế có thể khác một chút tùy theo migrations đã chạy
- Để biết cấu trúc chính xác nhất, vui lòng kiểm tra database trực tiếp hoặc xem migrations history

**Contact:** Fox WMS Development Team
