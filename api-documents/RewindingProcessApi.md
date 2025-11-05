# API Công Đoạn Tua (Rewinding Process API)

API này cung cấp các endpoint để quản lý công đoạn tua trong quy trình sản xuất.

## Base URL

```
/api/rewinding-processes
```

## Authentication

Tất cả các endpoint yêu cầu xác thực bằng JWT token. Thêm token vào header:

```
Authorization: Bearer {your_token}
```

---

## 1. Lấy Danh Sách Công Đoạn Tua

### Endpoint

```http
GET /api/rewinding-processes
```

### Mô tả

Lấy danh sách tất cả công đoạn tua với khả năng phân trang, lọc và sắp xếp.

### Query Parameters (Gridify)

| Tên        | Kiểu    | Mô tả                                    | Ví dụ                             |
| ---------- | ------- | ---------------------------------------- | --------------------------------- |
| `page`     | integer | Số trang (mặc định: 1)                   | `?page=1`                         |
| `pageSize` | integer | Số bản ghi trên mỗi trang (mặc định: 10) | `?pageSize=20`                    |
| `filter`   | string  | Điều kiện lọc theo cú pháp Gridify       | `?filter=ProductionShift=="Ca 1"` |
| `orderBy`  | string  | Sắp xếp theo trường                      | `?orderBy=ProductionDate desc`    |

### Ví dụ Request

```http
GET /api/rewinding-processes?page=1&pageSize=10&filter=ProductionShift=="Ca 1"&orderBy=ProductionDate desc
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
      "totalRewindingOutput": 1200.5,
      "totalBlowingStageMold": 30.2,
      "totalRewindingStageMold": 20.5
    }
  ],
  "totalCount": 50,
  "page": 1,
  "pageSize": 10,
  "totalPages": 5
}
```

### Mô tả các trường Response

#### Thông tin Header (RewindingProcess)

| Trường                     | Kiểu     | Mô tả                        |
| -------------------------- | -------- | ---------------------------- |
| `id`                       | integer  | ID công đoạn tua             |
| `shiftLeaderId`            | integer  | ID trưởng ca                 |
| `shiftLeaderName`          | string   | Tên trưởng ca                |
| `isDraft`                  | bool     | Có phải bản nháp không       |
| `productionDate`           | datetime | Ngày sản xuất                |
| `productionShift`          | string   | Ca sản xuất                  |
| `totalRewindingOutput`     | decimal  | Tổng sản lượng tua (kg)      |
| `totalBlowingStageMold`    | decimal  | Tổng DC công đoạn Thổi (kg)  |
| `totalRewindingStageMold`  | decimal  | Tổng DC công đoạn Tua (kg)   |

---

## 2. Lấy Chi Tiết Công Đoạn Tua

### Endpoint

```http
GET /api/rewinding-processes/{id}
```

### Mô tả

Lấy thông tin chi tiết một công đoạn tua theo ID.

### Path Parameters

| Tên  | Kiểu    | Bắt buộc | Mô tả            |
| ---- | ------- | -------- | ---------------- |
| `id` | integer | Có       | ID công đoạn tua |

### Ví dụ Request

```http
GET /api/rewinding-processes/1
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
  "totalRewindingOutput": 1200.5,
  "totalBlowingStageMold": 30.2,
  "totalRewindingStageMold": 20.5,
  "lines": [
    {
      "id": 1,
      "rewindingProcessId": 1,
      "productionOrderId": 100,
      "itemCode": "PROD001",
      "productionBatch": "LOT-2025-001",
      "cardCode": "C001",
      "customerName": "Công ty ABC",
      "productType": "PE",
      "productTypeName": "PE Cắt",
      "thickness": "50 micron",
      "semiProductWidth": "1200mm",
      "rewindingMachine": "Máy Tua 01",
      "workerId": 10,
      "workerName": "Trần Văn B",
      "rewindingSpeed": 120.5,
      "startTime": "2025-10-29T07:00:00",
      "endTime": "2025-10-29T15:00:00",
      "machineStopMinutes": 30.0,
      "stopReason": "Thay dao",
      "rollCount": 50.0,
      "quantityKg": 1000.0,
      "boxCount": 20.0,
      "requiredDate": "2025-10-30T00:00:00",
      "isCompleted": true,
      "actualCompletionDate": "2025-10-29T15:00:00",
      "delayReason": null,
      "blowingLossKg": 10.0,
      "blowingLossReason": "DC từ công đoạn thổi",
      "humanLossKg": 5.0,
      "humanLossReason": "Nhầm lẫn cài đặt",
      "machineLossKg": 3.0,
      "machineLossReason": "Máy bị kẹt",
      "totalLossKg": 18.0,
      "excessPO": 10.0,
      "btpWarehouseConfirmed": false
    }
  ]
}
```

### Mô tả các trường Response

#### Thông tin Header (RewindingProcess)

| Trường                     | Kiểu     | Mô tả                        |
| -------------------------- | -------- | ---------------------------- |
| `id`                       | integer  | ID công đoạn tua             |
| `shiftLeaderId`            | integer  | ID trưởng ca                 |
| `shiftLeaderName`          | string   | Tên trưởng ca                |
| `isDraft`                  | bool     | Có phải bản nháp không       |
| `productionDate`           | datetime | Ngày sản xuất                |
| `productionShift`          | string   | Ca sản xuất                  |
| `totalRewindingOutput`     | decimal  | Tổng sản lượng tua (kg)      |
| `totalBlowingStageMold`    | decimal  | Tổng DC công đoạn Thổi (kg)  |
| `totalRewindingStageMold`  | decimal  | Tổng DC công đoạn Tua (kg)   |
| `lines`                    | array    | Danh sách chi tiết công đoạn tua |

#### Thông tin Chi tiết (RewindingProcessLine)

| Trường                   | Kiểu     | Mô tả                               |
| ------------------------ | -------- | ----------------------------------- |
| `id`                     | integer  | ID chi tiết công đoạn               |
| `rewindingProcessId`     | integer  | ID công đoạn tua                    |
| `productionOrderId`      | integer  | ID lệnh sản xuất SAP                |
| `itemCode`               | string   | Mã hàng                             |
| `productionBatch`        | string   | Lô sản xuất                         |
| `cardCode`               | string   | Mã khách hàng                       |
| `customerName`           | string   | Tên khách hàng                      |
| `productType`            | string   | Chủng loại                          |
| `productTypeName`        | string   | Tên chủng loại                      |
| `thickness`              | string   | Độ dày / 1 lá                       |
| `semiProductWidth`       | string   | Khổ màng BTP                        |
| `rewindingMachine`       | string   | Máy tua                             |
| `workerId`               | integer  | ID công nhân tua                    |
| `workerName`             | string   | Tên công nhân tua                   |
| `rewindingSpeed`         | decimal  | Tốc độ tua                          |
| `startTime`              | datetime | Thời gian bắt đầu tua               |
| `endTime`                | datetime | Thời gian kết thúc tua              |
| `machineStopMinutes`     | decimal  | Thời gian dừng máy (phút)           |
| `stopReason`             | string   | Nguyên nhân dừng máy                |
| `rollCount`              | decimal  | Số cuộn                             |
| `quantityKg`             | decimal  | Số kg                               |
| `boxCount`               | decimal  | Số thùng                            |
| `requiredDate`           | datetime | Ngày cần hàng                       |
| `isCompleted`            | boolean  | Xác nhận hoàn thành                 |
| `actualCompletionDate`   | datetime | Ngày hoàn thành thực tế (QLSX)      |
| `delayReason`            | string   | Nguyên nhân chậm tiến độ            |
| `blowingLossKg`          | decimal  | DC do công đoạn thổi (Kg)           |
| `blowingLossReason`      | string   | DC do công đoạn thổi - Nguyên nhân  |
| `humanLossKg`            | decimal  | DC do công đoạn tua - Con người (Kg)|
| `humanLossReason`        | string   | DC do công đoạn tua - Nguyên nhân con người |
| `machineLossKg`          | decimal  | DC do công đoạn tua - Lỗi máy (Kg)  |
| `machineLossReason`      | string   | DC do công đoạn tua - Nguyên nhân lỗi máy |
| `totalLossKg`            | decimal  | Tổng DC (Kg)                        |
| `excessPO`               | decimal  | Thừa PO (Kg)                        |
| `btpWarehouseConfirmed`  | boolean  | Xác nhận của kho BTP                |

### Response Error (404 Not Found)

```json
{
  "message": "Không tìm thấy công đoạn tua với ID: 1",
  "statusCode": 404
}
```

---

## 3. Tạo Công Đoạn Tua Mới

### Endpoint

```http
POST /api/rewinding-processes
```

### Mô tả

Tạo một công đoạn tua mới. Trưởng ca sẽ tự động được gán từ người dùng hiện tại.

### Request Body

```json
{
  "isDraft": false,
  "productionDate": "2025-10-29T00:00:00",
  "productionShift": "Ca 1",
  "lines": [
    {
      "productionOrderId": 100,
      "rewindingMachine": "Máy Tua 01",
      "workerId": 10,
      "rewindingSpeed": 120.5,
      "startTime": "2025-10-29T07:00:00",
      "endTime": "2025-10-29T15:00:00",
      "machineStopMinutes": 30.0,
      "stopReason": "Thay dao",
      "rollCount": 50.0,
      "quantityKg": 1000.0,
      "boxCount": 20.0,
      "isCompleted": true,
      "actualCompletionDate": "2025-10-29T15:00:00",
      "delayReason": null,
      "blowingLossKg": 10.0,
      "blowingLossReason": "DC từ công đoạn thổi",
      "humanLossKg": 5.0,
      "humanLossReason": "Nhầm lẫn cài đặt",
      "machineLossKg": 3.0,
      "machineLossReason": "Máy bị kẹt",
      "excessPO": 10.0,
      "btpWarehouseConfirmed": false
    }
  ]
}
```

### Mô tả các trường Request (CreateRewindingProcessDto)

#### Thông tin Header

| Trường            | Kiểu     | Bắt buộc | Giới hạn     | Mô tả                          |
| ----------------- | -------- | -------- | ------------ | ------------------------------ |
| `isDraft`         | boolean  | Không    | -            | Có phải bản nháp không         |
| `productionDate`  | datetime | **Có**   | -            | Ngày sản xuất                  |
| `productionShift` | string   | **Có**   | Max 50 ký tự | Ca sản xuất                    |
| `lines`           | array    | Không    | -            | Danh sách chi tiết công đoạn tua |

#### Thông tin Chi tiết (CreateRewindingProcessLineDto)

| Trường                  | Kiểu     | Bắt buộc | Giới hạn      | Mô tả                                    |
| ----------------------- | -------- | -------- | ------------- | ---------------------------------------- |
| `productionOrderId`     | integer  | **Có**   | -             | DocEntry của lệnh sản xuất               |
| `rewindingMachine`      | string   | Không    | Max 50 ký tự  | Máy tua                                  |
| `workerId`              | integer  | Không    | -             | ID công nhân tua                         |
| `rewindingSpeed`        | decimal  | Không    | >= 0          | Tốc độ tua                               |
| `startTime`             | datetime | Không    | -             | Thời gian bắt đầu tua                    |
| `endTime`               | datetime | Không    | -             | Thời gian kết thúc tua                   |
| `machineStopMinutes`    | decimal  | Không    | >= 0          | Thời gian dừng máy (phút)                |
| `stopReason`            | string   | Không    | Max 500 ký tự | Nguyên nhân dừng máy                     |
| `rollCount`             | decimal  | Không    | >= 0          | Số cuộn                                  |
| `quantityKg`            | decimal  | Không    | >= 0          | Số kg                                    |
| `boxCount`              | decimal  | Không    | >= 0          | Số thùng                                 |
| `isCompleted`           | boolean  | Không    | -             | Xác nhận hoàn thành                      |
| `actualCompletionDate`  | datetime | Không    | -             | Ngày hoàn thành thực tế (QLSX)           |
| `delayReason`           | string   | Không    | Max 500 ký tự | Nguyên nhân chậm tiến độ                 |
| `blowingLossKg`         | decimal  | Không    | >= 0          | DC do công đoạn thổi (Kg)                |
| `blowingLossReason`     | string   | Không    | Max 500 ký tú | DC do công đoạn thổi - Nguyên nhân       |
| `humanLossKg`           | decimal  | Không    | >= 0          | DC do công đoạn tua - Con người (Kg)     |
| `humanLossReason`       | string   | Không    | Max 500 ký tự | DC do công đoạn tua - Nguyên nhân con người |
| `machineLossKg`         | decimal  | Không    | >= 0          | DC do công đoạn tua - Lỗi máy (Kg)       |
| `machineLossReason`     | string   | Không    | Max 500 ký tự | DC do công đoạn tua - Nguyên nhân lỗi máy |
| `excessPO`              | decimal  | Không    | >= 0          | Thừa PO (Kg)                             |
| `btpWarehouseConfirmed` | boolean  | Không    | -             | Xác nhận của kho BTP                     |

### Ví dụ Request

```http
POST /api/rewinding-processes
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
Content-Type: application/json

{
  "productionDate": "2025-10-29T00:00:00",
  "productionShift": "Ca 1",
  "lines": [
    {
      "productionOrderId": 100,
      "quantityKg": 1000.0
    }
  ]
}
```

### Response Success (200 OK)

Trả về đối tượng `RewindingProcess` vừa được tạo (xem cấu trúc ở phần 2).

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

## 4. Cập Nhật Công Đoạn Tua

### Endpoint

```http
PUT /api/rewinding-processes/{id}
```

### Mô tả

Cập nhật thông tin công đoạn tua hiện có.

### Path Parameters

| Tên  | Kiểu    | Bắt buộc | Mô tả                        |
| ---- | ------- | -------- | ---------------------------- |
| `id` | integer | Có       | ID công đoạn tua cần cập nhật |

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
      "rewindingMachine": "Máy Tua 01",
      "workerId": 10,
      "rewindingSpeed": 120.5,
      "startTime": "2025-10-29T07:00:00",
      "endTime": "2025-10-29T15:00:00",
      "machineStopMinutes": 30.0,
      "stopReason": "Thay dao",
      "rollCount": 50.0,
      "quantityKg": 1000.0,
      "boxCount": 20.0,
      "isCompleted": true,
      "actualCompletionDate": "2025-10-29T15:00:00",
      "delayReason": null,
      "blowingLossKg": 10.0,
      "blowingLossReason": "DC từ công đoạn thổi",
      "humanLossKg": 5.0,
      "humanLossReason": "Nhầm lẫn cài đặt",
      "machineLossKg": 3.0,
      "machineLossReason": "Máy bị kẹt",
      "excessPO": 10.0,
      "btpWarehouseConfirmed": false
    }
  ]
}
```

### Mô tả các trường Request (UpdateRewindingProcessDto)

#### Thông tin Header

| Trường            | Kiểu     | Bắt buộc | Giới hạn     | Mô tả                          |
| ----------------- | -------- | -------- | ------------ | ------------------------------ |
| `isDraft`         | boolean  | Không    | -            | Có phải bản nháp không         |
| `productionDate`  | datetime | **Có**   | -            | Ngày sản xuất                  |
| `productionShift` | string   | **Có**   | Max 50 ký tự | Ca sản xuất                    |
| `lines`           | array    | Không    | -            | Danh sách chi tiết công đoạn tua |

#### Thông tin Chi tiết (UpdateRewindingProcessLineDto)

| Trường                  | Kiểu     | Bắt buộc | Giới hạn      | Mô tả                                           |
| ----------------------- | -------- | -------- | ------------- | ----------------------------------------------- |
| `id`                    | integer  | Không    | -             | ID của line (null nếu là line mới)              |
| `productionOrderId`     | integer  | **Có**   | -             | DocEntry của lệnh sản xuất                      |
| `rewindingMachine`      | string   | Không    | Max 50 ký tự  | Máy tua                                         |
| `workerId`              | integer  | Không    | -             | ID công nhân tua                                |
| `rewindingSpeed`        | decimal  | Không    | >= 0          | Tốc độ tua                                      |
| `startTime`             | datetime | Không    | -             | Thời gian bắt đầu tua                           |
| `endTime`               | datetime | Không    | -             | Thời gian kết thúc tua                          |
| `machineStopMinutes`    | decimal  | Không    | >= 0          | Thời gian dừng máy (phút)                       |
| `stopReason`            | string   | Không    | Max 500 ký tự | Nguyên nhân dừng máy                            |
| `rollCount`             | decimal  | Không    | >= 0          | Số cuộn                                         |
| `quantityKg`            | decimal  | Không    | >= 0          | Số kg                                           |
| `boxCount`              | decimal  | Không    | >= 0          | Số thùng                                        |
| `isCompleted`           | boolean  | Không    | -             | Xác nhận hoàn thành                             |
| `actualCompletionDate`  | datetime | Không    | -             | Ngày hoàn thành thực tế (QLSX)                  |
| `delayReason`           | string   | Không    | Max 500 ký tự | Nguyên nhân chậm tiến độ                        |
| `blowingLossKg`         | decimal  | Không    | >= 0          | DC do công đoạn thổi (Kg)                       |
| `blowingLossReason`     | string   | Không    | Max 500 ký tự | DC do công đoạn thổi - Nguyên nhân              |
| `humanLossKg`           | decimal  | Không    | >= 0          | DC do công đoạn tua - Con người (Kg)            |
| `humanLossReason`       | string   | Không    | Max 500 ký tự | DC do công đoạn tua - Nguyên nhân con người     |
| `machineLossKg`         | decimal  | Không    | >= 0          | DC do công đoạn tua - Lỗi máy (Kg)              |
| `machineLossReason`     | string   | Không    | Max 500 ký tự | DC do công đoạn tua - Nguyên nhân lỗi máy       |
| `excessPO`              | decimal  | Không    | >= 0          | Thừa PO (Kg)                                    |
| `btpWarehouseConfirmed` | boolean  | Không    | -             | Xác nhận của kho BTP                            |

**Lưu ý:** 
- Nếu `id` có giá trị, line sẽ được cập nhật.
- Nếu `id` là `null`, line mới sẽ được thêm vào.
- Các line không có trong request sẽ bị xóa.

### Ví dụ Request

```http
PUT /api/rewinding-processes/1
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
Content-Type: application/json

{
  "productionDate": "2025-10-29T00:00:00",
  "productionShift": "Ca 2",
  "lines": [
    {
      "id": 1,
      "productionOrderId": 100,
      "quantityKg": 1200.0
    }
  ]
}
```

### Response Success (200 OK)

Trả về đối tượng `RewindingProcess` sau khi cập nhật (xem cấu trúc ở phần 2).

### Response Error (404 Not Found)

```json
{
  "message": "Không tìm thấy công đoạn tua với ID: 1",
  "statusCode": 404
}
```

### Response Error (400 Bad Request)

```json
{
  "message": "Ca sản xuất là bắt buộc",
  "statusCode": 400
}
```

---

## 5. Xóa Công Đoạn Tua

### Endpoint

```http
DELETE /api/rewinding-processes/{id}
```

### Mô tả

Xóa một công đoạn tua theo ID. Các chi tiết công đoạn tua (lines) cũng sẽ bị xóa theo.

### Path Parameters

| Tên  | Kiểu    | Bắt buộc | Mô tả                   |
| ---- | ------- | -------- | ----------------------- |
| `id` | integer | Có       | ID công đoạn tua cần xóa |

### Ví dụ Request

```http
DELETE /api/rewinding-processes/1
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

### Response Success (204 No Content)

Không có nội dung trả về khi xóa thành công.

### Response Error (404 Not Found)

```json
{
  "message": "Không tìm thấy công đoạn tua với ID: 1",
  "statusCode": 404
}
```

---

## Lưu ý chung

### Tính toán tự động

Khi tạo hoặc cập nhật công đoạn tua, các trường sau sẽ được tự động tính toán:

1. **TotalLossKg (cho mỗi line):**
   ```
   TotalLossKg = BlowingLossKg + HumanLossKg + MachineLossKg
   ```

2. **TotalRewindingOutput (tổng sản lượng tua):**
   ```
   TotalRewindingOutput = Sum(QuantityKg của tất cả lines)
   ```

3. **TotalBlowingStageMold (tổng DC công đoạn Thổi):**
   ```
   TotalBlowingStageMold = Sum(BlowingLossKg của tất cả lines)
   ```

4. **TotalRewindingStageMold (tổng DC công đoạn Tua):**
   ```
   TotalRewindingStageMold = Sum(HumanLossKg + MachineLossKg của tất cả lines)
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

### Xác thực

- `ShiftLeaderId` tự động được gán từ `EmployeeId` của người dùng đang đăng nhập
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
