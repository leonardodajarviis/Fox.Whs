# API Công Đoạn In (Printing Process API)

API này cung cấp các endpoint để quản lý công đoạn in trong quy trình sản xuất.

## Base URL

```
/api/printing-processes
```

## Authentication

Tất cả các endpoint yêu cầu xác thực bằng JWT token. Thêm token vào header:

```
Authorization: Bearer {your_token}
```

---

## 1. Lấy Danh Sách Công Đoạn In

### Endpoint

```http
GET /api/printing-processes
```

### Mô tả

Lấy danh sách tất cả công đoạn in với khả năng phân trang, lọc và sắp xếp.

### Query Parameters (Gridify)

| Tên        | Kiểu    | Mô tả                                    | Ví dụ                             |
| ---------- | ------- | ---------------------------------------- | --------------------------------- |
| `page`     | integer | Số trang (mặc định: 1)                   | `?page=1`                         |
| `pageSize` | integer | Số bản ghi trên mỗi trang (mặc định: 10) | `?pageSize=20`                    |
| `filter`   | string  | Điều kiện lọc theo cú pháp Gridify       | `?filter=ProductionShift=="Ca 1"` |
| `orderBy`  | string  | Sắp xếp theo trường                      | `?orderBy=ProductionDate desc`    |

### Ví dụ Request

```http
GET /api/printing-processes?page=1&pageSize=10&filter=ProductionShift=="Ca 1"&orderBy=ProductionDate desc
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
      "productionDate": "2025-10-31T00:00:00",
      "productionShift": "Ca 1",
      "totalPrintingOutput": 1200.5,
      "totalProcessingMold": 50.0,
      "totalBlowingStageMold": 30.0,
      "totalPrintingStageMold": 45.5
    }
  ],
  "totalCount": 50,
  "page": 1,
  "pageSize": 10,
  "totalPages": 5
}
```

### Mô tả các trường Response

#### Thông tin Header (PrintingProcess)

| Trường                   | Kiểu     | Mô tả                       |
| ------------------------ | -------- | --------------------------- |
| `id`                     | integer  | ID công đoạn in             |
| `shiftLeaderId`          | integer  | ID trưởng ca                |
| `shiftLeaderName`        | string   | Tên trưởng ca               |
| `isDraft`                | bool     | Có phải bản nháp không      |
| `productionDate`         | datetime | Ngày sản xuất               |
| `productionShift`        | string   | Ca sản xuất                 |
| `totalPrintingOutput`    | decimal  | Tổng sản lượng in (kg)      |
| `totalProcessingMold`    | decimal  | Tổng DC gia công (kg)       |
| `totalBlowingStageMold`  | decimal  | Tổng DC công đoạn Thổi (kg) |
| `totalPrintingStageMold` | decimal  | Tổng DC công đoạn In (kg)   |

---

## 2. Lấy Chi Tiết Công Đoạn In

### Endpoint

```http
GET /api/printing-processes/{id}
```

### Mô tả

Lấy thông tin chi tiết một công đoạn in theo ID.

### Path Parameters

| Tên  | Kiểu    | Bắt buộc | Mô tả           |
| ---- | ------- | -------- | --------------- |
| `id` | integer | Có       | ID công đoạn in |

### Ví dụ Request

```http
GET /api/printing-processes/1
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

### Response Success (200 OK)

```json
{
  "id": 1,
  "shiftLeaderId": 5,
  "shiftLeaderName": "Nguyễn Văn A",
  "isDraft": false,
  "productionDate": "2025-10-31T00:00:00",
  "productionShift": "Ca 1",
  "totalPrintingOutput": 1200.5,
  "totalProcessingMold": 50.0,
  "totalBlowingStageMold": 30.0,
  "totalPrintingStageMold": 45.5,
  "lines": [
    {
      "id": 1,
      "productionOrderId": 1,
      "itemCode": "PROD001",
      "productionBatch": "LOT-2025-001",
      "cardCode": "C00001",
      "customerName": "Công ty ABC",
      "productType": "PE",
      "productTypeName": "PE Name",
      "thickness": "50",
      "semiProductWidth": "1200mm",
      "printPatternName": "Hình in ABC",
      "colorCount": "4 màu",
      "printingMachine": "Máy in 01",
      "workerId": 10,
      "workerName": "Trần Văn B",
      "printingSpeed": "80 m/phút",
      "startTime": "2025-10-31T07:00:00",
      "endTime": "2025-10-31T15:00:00",
      "machineStopMinutes": 30.0,
      "stopReason": "Thay khuôn in",
      "rollCount": 15,
      "pieceCount": 500,
      "QuantityKg": 800.0,
      "requiredDate": "2025-11-01",
      "isCompleted": true,
      "actualCompletionDate": "2025-10-31T15:00:00",
      "delayReason": null,
      "processingLossKg": 25.0,
      "processingLossReason": "DC gia công",
      "blowingLossKg": 15.0,
      "blowingLossReason": "DC từ công đoạn thổi",
      "oppRollHeadKg": 10.0,
      "oppRollHeadReason": "Đầu cuộn OPP lỗi",
      "humanLossKg": 8.0,
      "humanLossReason": "Lỗi thao tác",
      "machineLossKg": 7.5,
      "machineLossReason": "Máy lỗi",
      "totalLossKg": 65.5,
      "excessPO": 10.2,
      "btpWarehouseConfirmation": true,
      "printingStageInventoryKg": 350.0
    }
  ]
}
```

### Mô tả các trường Response

#### Thông tin Header (PrintingProcess)

| Trường                   | Kiểu     | Mô tả                           |
| ------------------------ | -------- | ------------------------------- |
| `id`                     | integer  | ID công đoạn in                 |
| `shiftLeaderId`          | integer  | ID trưởng ca                    |
| `shiftLeaderName`        | string   | Tên trưởng ca                   |
| `isDraft`                | bool     | Có phải bản nháp không          |
| `productionDate`         | datetime | Ngày sản xuất                   |
| `productionShift`        | string   | Ca sản xuất                     |
| `totalPrintingOutput`    | decimal  | Tổng sản lượng in (kg)          |
| `totalProcessingMold`    | decimal  | Tổng DC gia công (kg)           |
| `totalBlowingStageMold`  | decimal  | Tổng DC công đoạn Thổi (kg)     |
| `totalPrintingStageMold` | decimal  | Tổng DC công đoạn In (kg)       |
| `lines`                  | array    | Danh sách chi tiết công đoạn in |

#### Thông tin Chi tiết (PrintingProcessLine)

| Trường                     | Kiểu     | Mô tả                              |
| -------------------------- | -------- | ---------------------------------- |
| `id`                       | integer  | ID chi tiết công đoạn              |
| `productionOrderId`        | integer  | ID lệnh sản xuất SAP               |
| `itemCode`                 | string   | Mã hàng                            |
| `productionBatch`          | string   | Lô sản xuất                        |
| `cardCode`                 | string   | Mã khách hàng                      |
| `customerName`             | string   | Tên khách hàng                     |
| `productType`              | string   | Chủng loại                         |
| `productTypeName`          | string   | Tên chủng loại                     |
| `thickness`                | string   | Độ dày / 1 lá                      |
| `semiProductWidth`         | string   | Khổ màng BTP                       |
| `printPatternName`         | string   | Tên hình in                        |
| `colorCount`               | string   | Số màu in                          |
| `printingMachine`          | string   | Máy in                             |
| `workerId`                 | integer  | ID công nhân in                    |
| `workerName`               | string   | Tên công nhân in                   |
| `printingSpeed`            | string   | Tốc độ in                          |
| `startTime`                | datetime | Thời gian bắt đầu in               |
| `endTime`                  | datetime | Thời gian kết thúc in              |
| `machineStopMinutes`       | decimal  | Thời gian dừng máy (phút)          |
| `stopReason`               | string   | Nguyên nhân dừng máy               |
| `rollCount`                | integer  | Số cuộn                            |
| `pieceCount`               | integer  | Số chiếc                           |
| `QuantityKg`               | decimal  | Số kg                              |
| `requiredDate`             | string   | Ngày cần hàng                      |
| `isCompleted`              | boolean  | Xác nhận hoàn thành                |
| `actualCompletionDate`     | datetime | Ngày hoàn thành thực tế            |
| `delayReason`              | string   | Nguyên nhân chậm tiến độ           |
| `processingLossKg`         | decimal  | DC gia công (kg)                   |
| `processingLossReason`     | string   | DC gia công - nguyên nhân          |
| `blowingLossKg`            | decimal  | DC do công đoạn thổi (kg)          |
| `blowingLossReason`        | string   | DC do công đoạn thổi - nguyên nhân |
| `oppRollHeadKg`            | decimal  | Đầu cuộn OPP (kg)                  |
| `oppRollHeadReason`        | string   | Đầu cuộn OPP - nguyên nhân         |
| `humanLossKg`              | decimal  | Con người (kg)                     |
| `humanLossReason`          | string   | Con người - nguyên nhân            |
| `machineLossKg`            | decimal  | Lỗi máy (kg)                       |
| `machineLossReason`        | string   | Lỗi máy - nguyên nhân              |
| `totalLossKg`              | decimal  | Tổng DC (kg)                       |
| `excessPO`                 | decimal  | Thừa PO                            |
| `btpWarehouseConfirmation` | boolean  | Xác nhận của kho BTP               |
| `printingStageInventoryKg` | decimal  | Tồn kho ở công đoạn In (kg)        |

### Response Error (404 Not Found)

```json
{
  "message": "Không tìm thấy công đoạn in",
  "statusCode": 404
}
```

---

## 3. Tạo Công Đoạn In Mới

### Endpoint

```http
POST /api/printing-processes
```

### Mô tả

Tạo một công đoạn in mới.

### Request Body

```json
{
  "shiftLeaderId": 5,
  "isDraft": false,
  "productionDate": "2025-10-31T00:00:00",
  "productionShift": "Ca 1",
  "lines": [
    {
      "productionOrderId": 1,
      "printPatternName": "Hình in ABC",
      "colorCount": "4 màu",
      "printingMachine": "Máy in 01",
      "workerId": 10,
      "printingSpeed": "80 m/phút",
      "startTime": "2025-10-31T07:00:00",
      "endTime": "2025-10-31T15:00:00",
      "machineStopMinutes": 30.0,
      "stopReason": "Thay khuôn in",
      "rollCount": 15,
      "pieceCount": 500,
      "QuantityKg": 800.0,
      "isCompleted": true,
      "actualCompletionDate": "2025-10-31T15:00:00",
      "delayReason": null,
      "processingLossKg": 25.0,
      "processingLossReason": "DC gia công",
      "blowingLossKg": 15.0,
      "blowingLossReason": "DC từ công đoạn thổi",
      "oppRollHeadKg": 10.0,
      "oppRollHeadReason": "Đầu cuộn OPP lỗi",
      "humanLossKg": 8.0,
      "humanLossReason": "Lỗi thao tác",
      "machineLossKg": 7.5,
      "machineLossReason": "Máy lỗi",
      "excessPO": 10.2,
      "btpWarehouseConfirmation": true,
      "printingStageInventoryKg": 350.0
    }
  ]
}
```

### Mô tả các trường Request (CreatePrintingProcessDto)

#### Thông tin Header

| Trường            | Kiểu     | Bắt buộc | Giới hạn     | Mô tả                           |
| ----------------- | -------- | -------- | ------------ | ------------------------------- |
| `shiftLeaderId`   | integer  | **Có**   | -            | ID trưởng ca                    |
| `isDraft`         | boolean  | Không    | -            | Có phải bản nháp không          |
| `productionDate`  | datetime | **Có**   | -            | Ngày sản xuất                   |
| `productionShift` | string   | **Có**   | Max 50 ký tự | Ca sản xuất                     |
| `lines`           | array    | Không    | -            | Danh sách chi tiết công đoạn in |

#### Thông tin Chi tiết (CreatePrintingProcessLineDto)

| Trường                     | Kiểu     | Bắt buộc | Giới hạn      | Mô tả                              |
| -------------------------- | -------- | -------- | ------------- | ---------------------------------- |
| `productionOrderId`        | integer  | **Có**   | -             | DocEntry của lệnh sản xuất         |
| `printPatternName`         | string   | Không    | Max 200 ký tự | Tên hình in                        |
| `colorCount`               | string   | Không    | Max 50 ký tự  | Số màu in                          |
| `printingMachine`          | string   | Không    | Max 50 ký tự  | Máy in                             |
| `workerId`                 | integer  | Không    | -             | ID công nhân in                    |
| `printingSpeed`            | string   | Không    | Max 50 ký tự  | Tốc độ in                          |
| `startTime`                | datetime | Không    | -             | Thời gian bắt đầu in               |
| `endTime`                  | datetime | Không    | -             | Thời gian kết thúc in              |
| `machineStopMinutes`       | decimal  | Không    | >= 0          | Thời gian dừng máy (phút)          |
| `stopReason`               | string   | Không    | Max 500 ký tự | Nguyên nhân dừng máy               |
| `rollCount`                | integer  | Không    | >= 0          | Số cuộn                            |
| `pieceCount`               | integer  | Không    | >= 0          | Số chiếc                           |
| `QuantityKg`               | decimal  | Không    | >= 0          | Số kg                              |
| `isCompleted`              | boolean  | Không    | -             | Xác nhận hoàn thành                |
| `actualCompletionDate`     | datetime | Không    | -             | Ngày hoàn thành thực tế            |
| `delayReason`              | string   | Không    | Max 500 ký tự | Nguyên nhân chậm tiến độ           |
| `processingLossKg`         | decimal  | Không    | >= 0          | DC gia công (kg)                   |
| `processingLossReason`     | string   | Không    | Max 500 ký tự | DC gia công - nguyên nhân          |
| `blowingLossKg`            | decimal  | Không    | >= 0          | DC do công đoạn thổi (kg)          |
| `blowingLossReason`        | string   | Không    | Max 500 ký tự | DC do công đoạn thổi - nguyên nhân |
| `oppRollHeadKg`            | decimal  | Không    | >= 0          | Đầu cuộn OPP (kg)                  |
| `oppRollHeadReason`        | string   | Không    | Max 500 ký tự | Đầu cuộn OPP - nguyên nhân         |
| `humanLossKg`              | decimal  | Không    | >= 0          | Con người (kg)                     |
| `humanLossReason`          | string   | Không    | Max 500 ký tự | Con người - nguyên nhân            |
| `machineLossKg`            | decimal  | Không    | >= 0          | Lỗi máy (kg)                       |
| `machineLossReason`        | string   | Không    | Max 500 ký tự | Lỗi máy - nguyên nhân              |
| `excessPO`                 | decimal  | Không    | -             | Thừa PO                            |
| `btpWarehouseConfirmation` | boolean  | Không    | -             | Xác nhận của kho BTP               |
| `printingStageInventoryKg` | decimal  | Không    | >= 0          | Tồn kho ở công đoạn In (kg)        |

### Ví dụ Request

```http
POST /api/printing-processes
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
Content-Type: application/json

{
  "shiftLeaderId": 5,
  "productionDate": "2025-10-31T00:00:00",
  "productionShift": "Ca 1",
  "lines": [
    {
      "productionOrderId": 1,
      "QuantityKg": 800.0
    }
  ]
}
```

### Response Success (200 OK)

```json
{
  "success": true
}
```

### Response Error (400 Bad Request)

```json
{
  "message": "Dữ liệu không hợp lệ",
  "statusCode": 400,
  "errors": {
    "ShiftLeaderId": ["Trưởng ca là bắt buộc"],
    "ProductionDate": ["Ngày sản xuất là bắt buộc"],
    "ProductionShift": ["Ca sản xuất là bắt buộc"]
  }
}
```

### Response Error (404 Not Found)

```json
{
  "message": "Không tìm thấy trưởng ca với ID: 5",
  "statusCode": 404
}
```

---

## 4. Cập Nhật Công Đoạn In

### Endpoint

```http
PUT /api/printing-processes/{id}
```

### Mô tả

Cập nhật thông tin công đoạn in theo ID.

### Path Parameters

| Tên  | Kiểu    | Bắt buộc | Mô tả                        |
| ---- | ------- | -------- | ---------------------------- |
| `id` | integer | Có       | ID công đoạn in cần cập nhật |

### Request Body

```json
{
  "shiftLeaderId": 5,
  "isDraft": false,
  "productionDate": "2025-10-31T00:00:00",
  "productionShift": "Ca 1",
  "lines": [
    {
      "id": 1,
      "productionOrderId": 1,
      "productType": "PE",
      "semiProductWidth": "1200mm",
      "printPatternName": "Hình in ABC",
      "colorCount": "4 màu",
      "printingMachine": "Máy in 01",
      "workerId": 10,
      "printingSpeed": "80 m/phút",
      "startTime": "2025-10-31T07:00:00",
      "endTime": "2025-10-31T15:00:00",
      "machineStopMinutes": 30.0,
      "stopReason": "Thay khuôn in",
      "rollCount": 15,
      "pieceCount": 500,
      "QuantityKg": 850.0,
      "isCompleted": true,
      "actualCompletionDate": "2025-10-31T15:00:00",
      "delayReason": null,
      "processingLossKg": 25.0,
      "processingLossReason": "DC gia công",
      "blowingLossKg": 15.0,
      "blowingLossReason": "DC từ công đoạn thổi",
      "oppRollHeadKg": 10.0,
      "oppRollHeadReason": "Đầu cuộn OPP lỗi",
      "humanLossKg": 8.0,
      "humanLossReason": "Lỗi thao tác",
      "machineLossKg": 7.5,
      "machineLossReason": "Máy lỗi",
      "excessPO": 10.2,
      "btpWarehouseConfirmation": true,
      "printingStageInventoryKg": 350.0
    },
    {
      "id": null,
      "productionOrderId": 2,
      "QuantityKg": 600.0
    }
  ]
}
```

### Mô tả các trường Request (UpdatePrintingProcessDto)

#### Thông tin Header

| Trường            | Kiểu     | Bắt buộc | Giới hạn     | Mô tả                           |
| ----------------- | -------- | -------- | ------------ | ------------------------------- |
| `shiftLeaderId`   | integer  | **Có**   | -            | ID trưởng ca                    |
| `isDraft`         | boolean  | Không    | -            | Có phải bản nháp không          |
| `productionDate`  | datetime | **Có**   | -            | Ngày sản xuất                   |
| `productionShift` | string   | **Có**   | Max 50 ký tự | Ca sản xuất                     |
| `lines`           | array    | Không    | -            | Danh sách chi tiết công đoạn in |

#### Thông tin Chi tiết (UpdatePrintingProcessLineDto)

| Trường                     | Kiểu     | Bắt buộc | Giới hạn      | Mô tả                                        |
| -------------------------- | -------- | -------- | ------------- | -------------------------------------------- |
| `id`                       | integer  | Không    | -             | ID chi tiết công đoạn (null nếu là dòng mới) |
| `productionOrderId`        | integer  | **Có**   | -             | ID lệnh sản xuất SAP                         |
| `productType`              | string   | Không    | Max 100 ký tự | Chủng loại                                   |
| `semiProductWidth`         | string   | Không    | Max 50 ký tự  | Khổ màng BTP                                 |
| `printPatternName`         | string   | Không    | Max 200 ký tự | Tên hình in                                  |
| `colorCount`               | string   | Không    | Max 50 ký tự  | Số màu in                                    |
| `printingMachine`          | string   | Không    | Max 50 ký tự  | Máy in                                       |
| `workerId`                 | integer  | Không    | -             | ID công nhân in                              |
| `printingSpeed`            | string   | Không    | Max 50 ký tự  | Tốc độ in                                    |
| `startTime`                | datetime | Không    | -             | Thời gian bắt đầu in                         |
| `endTime`                  | datetime | Không    | -             | Thời gian kết thúc in                        |
| `machineStopMinutes`       | decimal  | Không    | >= 0          | Thời gian dừng máy (phút)                    |
| `stopReason`               | string   | Không    | Max 500 ký tự | Nguyên nhân dừng máy                         |
| `rollCount`                | integer  | Không    | >= 0          | Số cuộn                                      |
| `pieceCount`               | integer  | Không    | >= 0          | Số chiếc                                     |
| `QuantityKg`               | decimal  | Không    | >= 0          | Số kg                                        |
| `isCompleted`              | boolean  | Không    | -             | Xác nhận hoàn thành                          |
| `actualCompletionDate`     | datetime | Không    | -             | Ngày hoàn thành thực tế                      |
| `delayReason`              | string   | Không    | Max 500 ký tự | Nguyên nhân chậm tiến độ                     |
| `processingLossKg`         | decimal  | Không    | >= 0          | DC gia công (kg)                             |
| `processingLossReason`     | string   | Không    | Max 500 ký tự | DC gia công - nguyên nhân                    |
| `blowingLossKg`            | decimal  | Không    | >= 0          | DC do công đoạn thổi (kg)                    |
| `blowingLossReason`        | string   | Không    | Max 500 ký tự | DC do công đoạn thổi - nguyên nhân           |
| `oppRollHeadKg`            | decimal  | Không    | >= 0          | Đầu cuộn OPP (kg)                            |
| `oppRollHeadReason`        | string   | Không    | Max 500 ký tú | Đầu cuộn OPP - nguyên nhân                   |
| `humanLossKg`              | decimal  | Không    | >= 0          | Con người (kg)                               |
| `humanLossReason`          | string   | Không    | Max 500 ký tự | Con người - nguyên nhân                      |
| `machineLossKg`            | decimal  | Không    | >= 0          | Lỗi máy (kg)                                 |
| `machineLossReason`        | string   | Không    | Max 500 ký tự | Lỗi máy - nguyên nhân                        |
| `excessPO`                 | decimal  | Không    | -             | Thừa PO                                      |
| `btpWarehouseConfirmation` | boolean  | Không    | -             | Xác nhận của kho BTP                         |
| `printingStageInventoryKg` | decimal  | Không    | >= 0          | Tồn kho ở công đoạn In (kg)                  |

**Lưu ý:**

- Nếu `id` của line là `null`, dòng đó sẽ được thêm mới
- Nếu `id` của line có giá trị, dòng đó sẽ được cập nhật
- Các dòng có trong database nhưng không có trong request sẽ bị xóa

### Ví dụ Request

```http
PUT /api/printing-processes/1
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
Content-Type: application/json

{
  "shiftLeaderId": 5,
  "productionDate": "2025-10-31T00:00:00",
  "productionShift": "Ca 1",
  "lines": [
    {
      "id": 1,
      "productionOrderId": 1,
      "QuantityKg": 850.0
    }
  ]
}
```

### Response Success (200 OK)

```json
{
  "success": true
}
```

### Response Error (404 Not Found)

```json
{
  "message": "Không tìm thấy công đoạn in với ID: 1",
  "statusCode": 404
}
```

### Response Error (400 Bad Request)

```json
{
  "message": "Dữ liệu không hợp lệ",
  "statusCode": 400,
  "errors": {
    "ShiftLeaderId": ["Trưởng ca là bắt buộc"],
    "Lines[0].ProductionOrderId": ["ID lệnh sản xuất là bắt buộc"]
  }
}
```

---

## 5. Xóa Công Đoạn In

### Endpoint

```http
DELETE /api/printing-processes/{id}
```

### Mô tả

Xóa một công đoạn in theo ID. Tất cả các chi tiết công đoạn in liên quan cũng sẽ bị xóa.

### Path Parameters

| Tên  | Kiểu    | Bắt buộc | Mô tả                   |
| ---- | ------- | -------- | ----------------------- |
| `id` | integer | Có       | ID công đoạn in cần xóa |

### Ví dụ Request

```http
DELETE /api/printing-processes/1
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

### Response Success (204 No Content)

```
(Không có nội dung trả về)
```

### Response Error (404 Not Found)

```json
{
  "message": "Không tìm thấy công đoạn in với ID: 1",
  "statusCode": 404
}
```

---

## Mã Lỗi Chung

| Mã lỗi | Ý nghĩa                                    |
| ------ | ------------------------------------------ |
| 200    | Thành công                                 |
| 204    | Thành công, không có nội dung trả về       |
| 400    | Yêu cầu không hợp lệ (Bad Request)         |
| 401    | Chưa xác thực (Unauthorized)               |
| 403    | Không có quyền truy cập (Forbidden)        |
| 404    | Không tìm thấy tài nguyên (Not Found)      |
| 409    | Xung đột dữ liệu (Conflict)                |
| 500    | Lỗi máy chủ nội bộ (Internal Server Error) |

---

## Ví dụ Sử dụng với cURL

### 1. Lấy danh sách công đoạn in

```bash
curl -X GET "http://localhost:5000/api/printing-processes?page=1&pageSize=10" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"
```

### 2. Lấy chi tiết công đoạn in

```bash
curl -X GET "http://localhost:5000/api/printing-processes/1" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"
```

### 3. Tạo công đoạn in mới

```bash
curl -X POST "http://localhost:5000/api/printing-processes" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE" \
  -H "Content-Type: application/json" \
  -d '{
    "shiftLeaderId": 5,
    "productionDate": "2025-10-31T00:00:00",
    "productionShift": "Ca 1",
    "lines": [
      {
        "productionOrderId": 1,
        "QuantityKg": 800.0
      }
    ]
  }'
```

### 4. Cập nhật công đoạn in

```bash
curl -X PUT "http://localhost:5000/api/printing-processes/1" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE" \
  -H "Content-Type: application/json" \
  -d '{
    "shiftLeaderId": 5,
    "productionDate": "2025-10-31T00:00:00",
    "productionShift": "Ca 1",
    "lines": [
      {
        "id": 1,
        "productionOrderId": 1,
        "QuantityKg": 850.0
      }
    ]
  }'
```

### 5. Xóa công đoạn in

```bash
curl -X DELETE "http://localhost:5000/api/printing-processes/1" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"
```

---

## Ví dụ Sử dụng với JavaScript (Fetch API)

### 1. Lấy danh sách công đoạn in

```javascript
const token = "YOUR_TOKEN_HERE";

fetch("http://localhost:5000/api/printing-processes?page=1&pageSize=10", {
  method: "GET",
  headers: {
    Authorization: `Bearer ${token}`,
  },
})
  .then((response) => response.json())
  .then((data) => console.log(data))
  .catch((error) => console.error("Error:", error));
```

### 2. Tạo công đoạn in mới

```javascript
const token = "YOUR_TOKEN_HERE";
const data = {
  shiftLeaderId: 5,
  productionDate: "2025-10-31T00:00:00",
  productionShift: "Ca 1",
  lines: [
    {
      productionOrderId: 1,
      QuantityKg: 800.0,
    },
  ],
};

fetch("http://localhost:5000/api/printing-processes", {
  method: "POST",
  headers: {
    Authorization: `Bearer ${token}`,
    "Content-Type": "application/json",
  },
  body: JSON.stringify(data),
})
  .then((response) => response.json())
  .then((data) => console.log(data))
  .catch((error) => console.error("Error:", error));
```

### 3. Cập nhật công đoạn in

```javascript
const token = "YOUR_TOKEN_HERE";
const data = {
  shiftLeaderId: 5,
  productionDate: "2025-10-31T00:00:00",
  productionShift: "Ca 1",
  lines: [
    {
      id: 1,
      productionOrderId: 1,
      QuantityKg: 850.0,
    },
  ],
};

fetch("http://localhost:5000/api/printing-processes/1", {
  method: "PUT",
  headers: {
    Authorization: `Bearer ${token}`,
    "Content-Type": "application/json",
  },
  body: JSON.stringify(data),
})
  .then((response) => response.json())
  .then((data) => console.log(data))
  .catch((error) => console.error("Error:", error));
```

### 4. Xóa công đoạn in

```javascript
const token = "YOUR_TOKEN_HERE";

fetch("http://localhost:5000/api/printing-processes/1", {
  method: "DELETE",
  headers: {
    Authorization: `Bearer ${token}`,
  },
})
  .then((response) => {
    if (response.status === 204) {
      console.log("Xóa thành công");
    }
  })
  .catch((error) => console.error("Error:", error));
```

---

## Lưu Ý Quan Trọng

1. **Xác thực**: Tất cả các API đều yêu cầu JWT token hợp lệ trong header Authorization.

2. **Phân trang**: API GetAll hỗ trợ phân trang với Gridify. Có thể lọc, sắp xếp theo nhiều trường khác nhau.

3. **Validation**: Các trường bắt buộc phải được cung cấp, nếu không API sẽ trả về lỗi 400 Bad Request.

4. **Tính toán tự động**: Các tổng (totalPrintingOutput, totalProcessingMold, totalBlowingStageMold, totalPrintingStageMold, totalLossKg) sẽ được tính toán tự động dựa trên dữ liệu chi tiết.

5. **Cập nhật Lines**:

   - Lines với `id = null` sẽ được thêm mới
   - Lines với `id` có giá trị sẽ được cập nhật
   - Lines không có trong request sẽ bị xóa

6. **Kiểu dữ liệu DateTime**: Sử dụng định dạng ISO 8601 (ví dụ: `2025-10-31T00:00:00` hoặc `2025-10-31T15:30:00Z`).

7. **Kiểu dữ liệu Decimal**: Sử dụng số thập phân với dấu chấm (.), không dùng dấu phẩy (,). Ví dụ: `800.50` thay vì `800,50`.

8. **Tồn kho công đoạn In**: Trường `printingStageInventoryKg` lưu trữ số lượng tồn kho ở công đoạn in.

---

## Thuật Ngữ & Viết Tắt

| Thuật ngữ | Ý nghĩa                                 |
| --------- | --------------------------------------- |
| **DC**    | Defective/Damage - Sản phẩm lỗi/hư hỏng |
| **BTP**   | Bán Thành Phẩm                          |
| **PO**    | Production Order - Đơn hàng sản xuất    |
| **OPP**   | Oriented Polypropylene - Loại màng nhựa |
| **PE**    | Polyethylene - Loại nhựa                |

---

**Phiên bản tài liệu:** 1.0  
**Ngày cập nhật:** 31/10/2025
