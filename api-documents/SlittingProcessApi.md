# API Công Đoạn Chia (Slitting Process API)

API này cung cấp các endpoint để quản lý công đoạn chia trong quy trình sản xuất.

## Base URL

```
/api/slitting-processes
```

## Authentication

Tất cả các endpoint yêu cầu xác thực bằng JWT token. Thêm token vào header:

```
Authorization: Bearer {your_token}
```

---

## 1. Lấy Danh Sách Công Đoạn Chia

### Endpoint

```http
GET /api/slitting-processes
```

### Mô tả

Lấy danh sách tất cả công đoạn chia với khả năng phân trang, lọc và sắp xếp.

### Query Parameters (Gridify)

| Tên        | Kiểu    | Mô tả                                    | Ví dụ                             |
| ---------- | ------- | ---------------------------------------- | --------------------------------- |
| `page`     | integer | Số trang (mặc định: 1)                   | `?page=1`                         |
| `pageSize` | integer | Số bản ghi trên mỗi trang (mặc định: 10) | `?pageSize=20`                    |
| `filter`   | string  | Điều kiện lọc theo cú pháp Gridify       | `?filter=ProductionShift=="Ca 1"` |
| `orderBy`  | string  | Sắp xếp theo trường                      | `?orderBy=ProductionDate desc`    |

### Ví dụ Request

```http
GET /api/slitting-processes?page=1&pageSize=10&filter=ProductionShift=="Ca 1"&orderBy=ProductionDate desc
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

### Response Success (200 OK)

```json
{
  "data": [
    {
      "id": 1,
      "shiftLeaderId": 5,
      "shiftLeaderName": "Nguyễn Văn A",
      "isDraft": false,
      "productionDate": "2025-10-29T00:00:00",
      "productionShift": "Ca 1",
      "totalProcessingMold": 15.5,
      "totalBlowingStageMold": 10.2,
      "totalPrintingStageMold": 8.3,
      "totalSlittingStageMold": 5.0
    }
  ],
  "totalCount": 50,
  "page": 1,
  "pageSize": 10,
  "totalPages": 5
}
```

### Mô tả các trường Response

#### Thông tin Header (SlittingProcess)

| Trường                    | Kiểu     | Mô tả                        |
| ------------------------- | -------- | ---------------------------- |
| `id`                      | integer  | ID công đoạn chia            |
| `shiftLeaderId`           | integer  | ID trưởng ca                 |
| `shiftLeaderName`         | string   | Tên trưởng ca                |
| `isDraft`                 | bool     | Có phải bản nháp không       |
| `productionDate`          | datetime | Ngày sản xuất                |
| `productionShift`         | string   | Ca sản xuất                  |
| `totalProcessingMold`     | decimal  | Tổng DC gia công (kg)        |
| `totalBlowingStageMold`   | decimal  | Tổng DC công đoạn Thổi (kg)  |
| `totalPrintingStageMold`  | decimal  | Tổng DC công đoạn In (kg)    |
| `totalSlittingStageMold`  | decimal  | Tổng DC công đoạn Chia (kg)  |

---

## 2. Lấy Chi Tiết Công Đoạn Chia

### Endpoint

```http
GET /api/slitting-processes/{id}
```

### Mô tả

Lấy thông tin chi tiết một công đoạn chia theo ID.

### Path Parameters

| Tên  | Kiểu    | Bắt buộc | Mô tả            |
| ---- | ------- | -------- | ---------------- |
| `id` | integer | Có       | ID công đoạn chia |

### Ví dụ Request

```http
GET /api/slitting-processes/1
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

### Response Success (200 OK)

```json
{
  "id": 1,
  "shiftLeaderId": 5,
  "shiftLeaderName": "Nguyễn Văn A",
  "isDraft": false,
  "productionDate": "2025-10-29T00:00:00",
  "productionShift": "Ca 1",
  "totalProcessingMold": 15.5,
  "totalBlowingStageMold": 10.2,
  "totalPrintingStageMold": 8.3,
  "totalSlittingStageMold": 5.0,
  "lines": [
    {
      "id": 1,
      "slittingProcessId": 1,
      "productionOrderId": 100,
      "itemCode": "PROD001",
      "productionBatch": "LOT-2025-001",
      "cardCode": "C001",
      "customerName": "Công ty ABC",
      "productType": "PE",
      "productTypeName": "PE In",
      "thickness": "50 micron",
      "semiProductWidth": "1200mm",
      "printPatternName": "Hoa văn A",
      "colorCount": "4 màu",
      "slittingMachine": "Máy Chia 01",
      "workerId": 10,
      "workerName": "Trần Văn B",
      "slittingSpeed": 100.5,
      "startTime": "2025-10-29T07:00:00",
      "endTime": "2025-10-29T15:00:00",
      "machineStopMinutes": 30.0,
      "stopReason": "Thay dao",
      "rollCount": 40.0,
      "pieceCount": 5000.0,
      "quantityKg": 800.0,
      "boxCount": 25.0,
      "requiredDate": "2025-10-30T00:00:00",
      "isCompleted": true,
      "actualCompletionDate": "2025-10-29T15:00:00",
      "delayReason": null,
      "processingLossKg": 5.0,
      "processingLossReason": "DC gia công",
      "blowingLossKg": 3.0,
      "blowingLossReason": "DC từ công đoạn thổi",
      "printingLossKg": 4.0,
      "printingLossReason": "DC từ công đoạn in",
      "printingMachine": "Máy In 02",
      "cutViaKg": 2.0,
      "humanLossKg": 2.5,
      "humanLossReason": "Nhầm lẫn cài đặt",
      "machineLossKg": 1.5,
      "machineLossReason": "Máy bị kẹt",
      "totalLossKg": 18.0,
      "excessPOPrinting": 5.0,
      "excessPOSlitting": 3.0,
      "btpWarehouseConfirmed": false
    }
  ]
}
```

### Mô tả các trường Response

#### Thông tin Header (SlittingProcess)

| Trường                    | Kiểu     | Mô tả                           |
| ------------------------- | -------- | ------------------------------- |
| `id`                      | integer  | ID công đoạn chia               |
| `shiftLeaderId`           | integer  | ID trưởng ca                    |
| `shiftLeaderName`         | string   | Tên trưởng ca                   |
| `isDraft`                 | bool     | Có phải bản nháp không          |
| `productionDate`          | datetime | Ngày sản xuất                   |
| `productionShift`         | string   | Ca sản xuất                     |
| `totalProcessingMold`     | decimal  | Tổng DC gia công (kg)           |
| `totalBlowingStageMold`   | decimal  | Tổng DC công đoạn Thổi (kg)     |
| `totalPrintingStageMold`  | decimal  | Tổng DC công đoạn In (kg)       |
| `totalSlittingStageMold`  | decimal  | Tổng DC công đoạn Chia (kg)     |
| `lines`                   | array    | Danh sách chi tiết công đoạn chia |

#### Thông tin Chi tiết (SlittingProcessLine)

| Trường                  | Kiểu     | Mô tả                                      |
| ----------------------- | -------- | ------------------------------------------ |
| `id`                    | integer  | ID chi tiết công đoạn                      |
| `slittingProcessId`     | integer  | ID công đoạn chia                          |
| `productionOrderId`     | integer  | ID lệnh sản xuất SAP                       |
| `itemCode`              | string   | Mã hàng                                    |
| `productionBatch`       | string   | Lô sản xuất                                |
| `cardCode`              | string   | Mã khách hàng                              |
| `customerName`          | string   | Tên khách hàng                             |
| `productType`           | string   | Chủng loại                                 |
| `productTypeName`       | string   | Tên chủng loại                             |
| `thickness`             | string   | Độ dày / 1 lá                              |
| `semiProductWidth`      | string   | Khổ màng BTP                               |
| `printPatternName`      | string   | Tên hình in                                |
| `colorCount`            | string   | Số màu in                                  |
| `slittingMachine`       | string   | Máy chia                                   |
| `workerId`              | integer  | ID công nhân chia                          |
| `workerName`            | string   | Tên công nhân chia                         |
| `slittingSpeed`         | decimal  | Tốc độ chia                                |
| `startTime`             | datetime | Thời gian bắt đầu chia                     |
| `endTime`               | datetime | Thời gian kết thúc chia                    |
| `machineStopMinutes`    | decimal  | Thời gian dừng máy (phút)                  |
| `stopReason`            | string   | Nguyên nhân dừng máy                       |
| `rollCount`             | decimal  | Số cuộn                                    |
| `pieceCount`            | decimal  | Số chiếc                                   |
| `quantityKg`            | decimal  | Số kg                                      |
| `boxCount`              | decimal  | Số thùng                                   |
| `requiredDate`          | datetime | Ngày cần hàng                              |
| `isCompleted`           | boolean  | Xác nhận hoàn thành                        |
| `actualCompletionDate`  | datetime | Ngày hoàn thành thực tế (QLSX)             |
| `delayReason`           | string   | Nguyên nhân chậm tiến độ                   |
| `processingLossKg`      | decimal  | DC gia công (Kg)                           |
| `processingLossReason`  | string   | DC gia công - Nguyên nhân                  |
| `blowingLossKg`         | decimal  | DC do công đoạn thổi (Kg)                  |
| `blowingLossReason`     | string   | DC do công đoạn thổi - Nguyên nhân         |
| `printingLossKg`        | decimal  | DC do công đoạn in (Kg)                    |
| `printingLossReason`    | string   | DC do công đoạn in - Nguyên nhân           |
| `printingMachine`       | string   | Số máy in                                  |
| `cutViaKg`              | decimal  | Cắt via (Kg)                               |
| `humanLossKg`           | decimal  | DC do công đoạn chia - Con người (Kg)      |
| `humanLossReason`       | string   | DC do công đoạn chia - Nguyên nhân con người |
| `machineLossKg`         | decimal  | DC do công đoạn chia - Lỗi máy (Kg)        |
| `machineLossReason`     | string   | DC do công đoạn chia - Nguyên nhân lỗi máy |
| `totalLossKg`           | decimal  | Tổng DC (Kg)                               |
| `excessPOPrinting`      | decimal  | Thừa PO - BTP In (Kg)                      |
| `excessPOSlitting`      | decimal  | Thừa PO - TP Chia (Kg)                     |
| `btpWarehouseConfirmed` | boolean  | Xác nhận của kho BTP                       |

### Response Error (404 Not Found)

```json
{
  "message": "Không tìm thấy công đoạn chia với ID: 1",
  "statusCode": 404
}
```

---

## 3. Tạo Công Đoạn Chia Mới

### Endpoint

```http
POST /api/slitting-processes
```

### Mô tả

Tạo một công đoạn chia mới. Trưởng ca sẽ tự động được gán từ người dùng hiện tại.

### Request Body

```json
{
  "isDraft": false,
  "productionDate": "2025-10-29T00:00:00",
  "productionShift": "Ca 1",
  "lines": [
    {
      "productionOrderId": 100,
      "slittingMachine": "Máy Chia 01",
      "workerId": 10,
      "slittingSpeed": 100.5,
      "startTime": "2025-10-29T07:00:00",
      "endTime": "2025-10-29T15:00:00",
      "machineStopMinutes": 30.0,
      "stopReason": "Thay dao",
      "rollCount": 40.0,
      "pieceCount": 5000.0,
      "quantityKg": 800.0,
      "boxCount": 25.0,
      "isCompleted": true,
      "actualCompletionDate": "2025-10-29T15:00:00",
      "delayReason": null,
      "processingLossKg": 5.0,
      "processingLossReason": "DC gia công",
      "blowingLossKg": 3.0,
      "blowingLossReason": "DC từ công đoạn thổi",
      "printingLossKg": 4.0,
      "printingLossReason": "DC từ công đoạn in",
      "printingMachine": "Máy In 02",
      "cutViaKg": 2.0,
      "humanLossKg": 2.5,
      "humanLossReason": "Nhầm lẫn cài đặt",
      "machineLossKg": 1.5,
      "machineLossReason": "Máy bị kẹt",
      "excessPOPrinting": 5.0,
      "excessPOSlitting": 3.0,
      "btpWarehouseConfirmed": false
    }
  ]
}
```

### Mô tả các trường Request (CreateSlittingProcessDto)

#### Thông tin Header

| Trường            | Kiểu     | Bắt buộc | Giới hạn     | Mô tả                          |
| ----------------- | -------- | -------- | ------------ | ------------------------------ |
| `isDraft`         | boolean  | Không    | -            | Có phải bản nháp không         |
| `productionDate`  | datetime | **Có**   | -            | Ngày sản xuất                  |
| `productionShift` | string   | **Có**   | Max 50 ký tự | Ca sản xuất                    |
| `lines`           | array    | Không    | -            | Danh sách chi tiết công đoạn chia |

#### Thông tin Chi tiết (CreateSlittingProcessLineDto)

| Trường                 | Kiểu     | Bắt buộc | Giới hạn      | Mô tả                                      |
| ---------------------- | -------- | -------- | ------------- | ------------------------------------------ |
| `productionOrderId`    | integer  | **Có**   | -             | DocEntry của lệnh sản xuất                 |
| `slittingMachine`      | string   | Không    | Max 50 ký tự  | Máy chia                                   |
| `workerId`             | integer  | Không    | -             | ID công nhân chia                          |
| `slittingSpeed`        | decimal  | Không    | >= 0          | Tốc độ chia                                |
| `startTime`            | datetime | Không    | -             | Thời gian bắt đầu chia                     |
| `endTime`              | datetime | Không    | -             | Thời gian kết thúc chia                    |
| `machineStopMinutes`   | decimal  | Không    | >= 0          | Thời gian dừng máy (phút)                  |
| `stopReason`           | string   | Không    | Max 500 ký tự | Nguyên nhân dừng máy                       |
| `rollCount`            | decimal  | Không    | >= 0          | Số cuộn                                    |
| `pieceCount`           | decimal  | Không    | >= 0          | Số chiếc                                   |
| `quantityKg`           | decimal  | Không    | >= 0          | Số kg                                      |
| `boxCount`             | decimal  | Không    | >= 0          | Số thùng                                   |
| `isCompleted`          | boolean  | Không    | -             | Xác nhận hoàn thành                        |
| `actualCompletionDate` | datetime | Không    | -             | Ngày hoàn thành thực tế (QLSX)             |
| `delayReason`          | string   | Không    | Max 500 ký tự | Nguyên nhân chậm tiến độ                   |
| `processingLossKg`     | decimal  | Không    | >= 0          | DC gia công (Kg)                           |
| `processingLossReason` | string   | Không    | Max 500 ký tự | DC gia công - Nguyên nhân                  |
| `blowingLossKg`        | decimal  | Không    | >= 0          | DC do công đoạn thổi (Kg)                  |
| `blowingLossReason`    | string   | Không    | Max 500 ký tự | DC do công đoạn thổi - Nguyên nhân         |
| `printingLossKg`       | decimal  | Không    | >= 0          | DC do công đoạn in (Kg)                    |
| `printingLossReason`   | string   | Không    | Max 500 ký tự | DC do công đoạn in - Nguyên nhân           |
| `printingMachine`      | string   | Không    | Max 50 ký tự  | Số máy in                                  |
| `cutViaKg`             | decimal  | Không    | >= 0          | Cắt via (Kg)                               |
| `humanLossKg`          | decimal  | Không    | >= 0          | DC do công đoạn chia - Con người (Kg)      |
| `humanLossReason`      | string   | Không    | Max 500 ký tự | DC do công đoạn chia - Nguyên nhân con người |
| `machineLossKg`        | decimal  | Không    | >= 0          | DC do công đoạn chia - Lỗi máy (Kg)        |
| `machineLossReason`    | string   | Không    | Max 500 ký tự | DC do công đoạn chia - Nguyên nhân lỗi máy |
| `excessPOPrinting`     | decimal  | Không    | >= 0          | Thừa PO - BTP In (Kg)                      |
| `excessPOSlitting`     | decimal  | Không    | >= 0          | Thừa PO - TP Chia (Kg)                     |
| `btpWarehouseConfirmed`| boolean  | Không    | -             | Xác nhận của kho BTP                       |

### Ví dụ Request

```http
POST /api/slitting-processes
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
Content-Type: application/json

{
  "productionDate": "2025-10-29T00:00:00",
  "productionShift": "Ca 1",
  "lines": [
    {
      "productionOrderId": 100,
      "quantityKg": 800.0
    }
  ]
}
```

### Response Success (200 OK)

Trả về đối tượng `SlittingProcess` vừa được tạo (xem cấu trúc ở phần 2).

### Response Error (400 Bad Request)

```json
{
  "message": "Ngày sản xuất là bắt buộc",
  "statusCode": 400
}
```

### Response Error (404 Not Found)

```json
{
  "message": "Không tìm thấy Production Order với ID: 100",
  "statusCode": 404
}
```

### Response Error (401 Unauthorized)

```json
{
  "message": "Không xác định được nhân viên hiện tại",
  "statusCode": 401
}
```

---

## 4. Cập Nhật Công Đoạn Chia

### Endpoint

```http
PUT /api/slitting-processes/{id}
```

### Mô tả

Cập nhật thông tin công đoạn chia hiện có.

### Path Parameters

| Tên  | Kiểu    | Bắt buộc | Mô tả                        |
| ---- | ------- | -------- | ---------------------------- |
| `id` | integer | Có       | ID công đoạn chia cần cập nhật |

### Request Body

```json
{
  "isDraft": false,
  "productionDate": "2025-10-29T00:00:00",
  "productionShift": "Ca 1",
  "lines": [
    {
      "id": 1,
      "productionOrderId": 100,
      "slittingMachine": "Máy Chia 01",
      "workerId": 10,
      "slittingSpeed": 100.5,
      "quantityKg": 850.0
    }
  ]
}
```

### Mô tả các trường Request (UpdateSlittingProcessDto)

Tương tự như `CreateSlittingProcessDto`, nhưng các line có thêm trường `id`:

#### Thông tin Chi tiết (UpdateSlittingProcessLineDto)

| Trường | Kiểu    | Bắt buộc | Mô tả                                  |
| ------ | ------- | -------- | -------------------------------------- |
| `id`   | integer | Không    | ID của line (null nếu là line mới)     |

**Lưu ý:** 
- Nếu `id` có giá trị, line sẽ được cập nhật.
- Nếu `id` là `null`, line mới sẽ được thêm vào.
- Các line không có trong request sẽ bị xóa.

### Response Success (200 OK)

Trả về đối tượng `SlittingProcess` sau khi cập nhật (xem cấu trúc ở phần 2).

### Response Error (404 Not Found)

```json
{
  "message": "Không tìm thấy công đoạn chia với ID: 1",
  "statusCode": 404
}
```

---

## 5. Xóa Công Đoạn Chia

### Endpoint

```http
DELETE /api/slitting-processes/{id}
```

### Mô tả

Xóa một công đoạn chia theo ID. Các chi tiết công đoạn chia (lines) cũng sẽ bị xóa theo.

### Path Parameters

| Tên  | Kiểu    | Bắt buộc | Mô tả                   |
| ---- | ------- | -------- | ----------------------- |
| `id` | integer | Có       | ID công đoạn chia cần xóa |

### Ví dụ Request

```http
DELETE /api/slitting-processes/1
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

### Response Success (204 No Content)

Không có nội dung trả về khi xóa thành công.

### Response Error (404 Not Found)

```json
{
  "message": "Không tìm thấy công đoạn chia với ID: 1",
  "statusCode": 404
}
```

---

## Lưu ý chung

### Tính toán tự động

Khi tạo hoặc cập nhật công đoạn chia, các trường sau sẽ được tự động tính toán:

1. **TotalLossKg (cho mỗi line):**
   ```
   TotalLossKg = ProcessingLossKg + BlowingLossKg + PrintingLossKg + 
                 CutViaKg + HumanLossKg + MachineLossKg
   ```

2. **TotalProcessingMold (tổng DC gia công):**
   ```
   TotalProcessingMold = Sum(ProcessingLossKg của tất cả lines)
   ```

3. **TotalBlowingStageMold (tổng DC công đoạn Thổi):**
   ```
   TotalBlowingStageMold = Sum(BlowingLossKg của tất cả lines)
   ```

4. **TotalPrintingStageMold (tổng DC công đoạn In):**
   ```
   TotalPrintingStageMold = Sum(PrintingLossKg + CutViaKg của tất cả lines)
   ```

5. **TotalSlittingStageMold (tổng DC công đoạn Chia):**
   ```
   TotalSlittingStageMold = Sum(HumanLossKg + MachineLossKg của tất cả lines)
   ```

### Thông tin tự động điền từ Production Order

Khi tạo hoặc cập nhật line với `productionOrderId`, các thông tin sau sẽ được tự động lấy từ SAP:

- `itemCode` - Mã hàng
- `cardCode` - Mã khách hàng
- `productionBatch` - Lô sản xuất
- `productType` - Chủng loại
- `productTypeName` - Tên chủng loại
- `thickness` - Độ dày
- `semiProductWidth` - Khổ màng BTP
- `printPatternName` - Tên hình in
- `colorCount` - Số màu in

### Xác thực

- `ShiftLeaderId` tự động được gán từ `EmployeeId` của người dùng đang đăng nhập
- `CreatorId` và `ModifierId` được tự động ghi nhận
- Tất cả các endpoint yêu cầu JWT token hợp lệ

### Gridify Filter Examples

Một số ví dụ filter hữu ích:

```
# Lọc theo ca sản xuất
?filter=ProductionShift=="Ca 1"

# Lọc theo ngày sản xuất
?filter=ProductionDate>=2025-10-01,ProductionDate<=2025-10-31

# Lọc theo trưởng ca
?filter=ShiftLeaderId==5

# Lọc bản nháp
?filter=IsDraft==true

# Lọc kết hợp
?filter=ProductionShift=="Ca 1",ProductionDate>=2025-10-01&orderBy=ProductionDate desc
```

### Mã lỗi

| Mã lỗi | Mô tả                                          |
| ------ | ---------------------------------------------- |
| 200    | Thành công                                     |
| 204    | Thành công - Không có nội dung trả về          |
| 400    | Yêu cầu không hợp lệ (validation error)        |
| 401    | Không được xác thực (token không hợp lệ)       |
| 403    | Không có quyền truy cập                        |
| 404    | Không tìm thấy tài nguyên                      |
| 500    | Lỗi server                                     |
