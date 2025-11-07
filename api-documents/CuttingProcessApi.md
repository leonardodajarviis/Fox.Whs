# API Công Đoạn Cắt (Cutting Process API)

API này cung cấp các endpoint để quản lý công đoạn cắt trong quy trình sản xuất.

## Base URL

```
/api/cutting-processes
```

## Authentication

Tất cả các endpoint yêu cầu xác thực bằng JWT token. Thêm token vào header:

```
Authorization: Bearer {your_token}
```

---

## 1. Lấy Danh Sách Công Đoạn Cắt

### Endpoint

```http
GET /api/cutting-processes
```

### Mô tả

Lấy danh sách tất cả công đoạn cắt với khả năng phân trang, lọc và sắp xếp.

### Query Parameters (Gridify)

| Tên        | Kiểu    | Mô tả                                    | Ví dụ                             |
| ---------- | ------- | ---------------------------------------- | --------------------------------- |
| `page`     | integer | Số trang (mặc định: 1)                   | `?page=1`                         |
| `pageSize` | integer | Số bản ghi trên mỗi trang (mặc định: 10) | `?pageSize=20`                    |
| `filter`   | string  | Điều kiện lọc theo cú pháp Gridify       | `?filter=ProductionShift=="Ca 1"` |
| `orderBy`  | string  | Sắp xếp theo trường                      | `?orderBy=ProductionDate desc`    |

### Ví dụ Request

```http
GET /api/cutting-processes?page=1&pageSize=10&filter=ProductionShift=="Ca 1"&orderBy=ProductionDate desc
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
      "productionDate": "2025-11-03T00:00:00",
      "productionShift": "Ca 1",
      "totalCuttingOutput": 2500.5,
      "totalFoldedCount": 1500.0,
      "totalProcessingMold": 75.0,
      "totalBlowingStageMold": 10.2,
      "totalPrintingStageMold": 8.3,
      "totalCuttingStageMold": 5.0
    }
  ],
  "totalCount": 50,
  "page": 1,
  "pageSize": 10,
  "totalPages": 5
}
```

### Mô tả các trường Response

#### Thông tin Header (CuttingProcess)

| Trường                   | Kiểu     | Mô tả                       |
| ------------------------ | -------- | --------------------------- |
| `id`                     | integer  | ID công đoạn cắt            |
| `shiftLeaderId`          | integer  | ID trưởng ca                |
| `shiftLeaderName`        | string   | Tên trưởng ca               |
| `isDraft`                | bool     | Có phải bản nháp không      |
| `productionDate`         | datetime | Ngày sản xuất               |
| `productionShift`        | string   | Ca sản xuất                 |
| `totalCuttingOutput`     | decimal  | Tổng sản lượng cắt (kg)     |
| `totalFoldedCount`       | decimal  | Tổng số lượng gấp xúc       |
| `totalProcessingMold`    | decimal  | Tổng DC gia công (kg)       |
| `totalBlowingStageMold`  | decimal  | Tổng DC công đoạn Thổi (kg) |
| `totalPrintingStageMold` | decimal  | Tổng DC công đoạn In (kg)   |
| `totalCuttingStageMold`  | decimal  | Tổng DC công đoạn Cắt (kg)  |

---

## 2. Lấy Chi Tiết Công Đoạn Cắt

### Endpoint

```http
GET /api/cutting-processes/{id}
```

### Mô tả

Lấy thông tin chi tiết một công đoạn cắt theo ID.

### Path Parameters

| Tên  | Kiểu    | Bắt buộc | Mô tả            |
| ---- | ------- | -------- | ---------------- |
| `id` | integer | Có       | ID công đoạn cắt |

### Ví dụ Request

```http
GET /api/cutting-processes/1
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

### Response Success (200 OK)

```json
{
  "id": 1,
  "shiftLeaderId": 5,
  "shiftLeaderName": "Nguyễn Văn A",
  "isDraft": false,
  "productionDate": "2025-11-03T00:00:00",
  "productionShift": "Ca 1",
  "totalCuttingOutput": 2500.5,
  "totalFoldedCount": 1500.0,
  "totalProcessingMold": 75.0,
  "totalBlowingStageMold": 10.2,
  "totalPrintingStageMold": 8.3,
  "totalCuttingStageMold": 5.0
  "lines": [
    {
      "id": 1,
      "productionOrderId": 1,
      "itemCode": "PROD001",
      "productionBatch": "LOT-2025-001",
      "cardCode": "C00001",
      "customerName": "Công ty ABC",
      "productType": "PE",
      "productTypeName": "PE Cắt",
      "thickness": "50",
      "semiProductWidth": "1200mm",
      "size": "30x40cm",
      "colorCount": "4 màu",
      "cuttingMachine": "Máy cắt 01",
      "workerId": 10,
      "workerName": "Trần Văn B",
      "cuttingSpeed": 120.0,
      "startTime": "2025-11-03T07:00:00",
      "endTime": "2025-11-03T15:00:00",
      "machineStopMinutes": 30.0,
      "stopReason": "Thay dao cắt",
      "pieceCount": 5000.0,
      "QuantityKg": 1200.0,
      "bagCount": 100.0,
      "foldedCount": 500.0,
      "requiredDate": "2025-11-04T00:00:00",
      "isCompleted": true,
      "actualCompletionDate": "2025-11-03T15:00:00",
      "delayReason": null,
      "processingLossKg": 30.0,
      "processingLossReason": "DC gia công",
      "blowingLossKg": 15.0,
      "blowingLossReason": "DC từ công đoạn thổi",
      "printingLossKg": 10.0,
      "printingLossReason": "DC từ công đoạn in",
      "printingMachine": "Máy in 02",
      "transferKg": 50.0,
      "humanLossKg": 8.0,
      "humanLossReason": "Lỗi thao tác",
      "machineLossKg": 7.0,
      "machineLossReason": "Máy lỗi",
      "totalLossKg": 70.0,
      "ExcessPOLess5Kg": 5.0,
      "ExcessPOOver5Kg": 10.0,
      "ExcessPOCut": 8.0,
      "btpWarehouseConfirmed": true,
      "remainingInventoryKg": 100.0
    }
  ]
}
```

### Mô tả các trường Response

#### Thông tin Header (CuttingProcess)

| Trường                   | Kiểu     | Mô tả                            |
| ------------------------ | -------- | -------------------------------- |
| `id`                     | integer  | ID công đoạn cắt                 |
| `shiftLeaderId`          | integer  | ID trưởng ca                     |
| `shiftLeaderName`        | string   | Tên trưởng ca                    |
| `isDraft`                | bool     | Có phải bản nháp không           |
| `productionDate`         | datetime | Ngày sản xuất                    |
| `productionShift`        | string   | Ca sản xuất                      |
| `totalCuttingOutput`     | decimal  | Tổng sản lượng cắt (kg)          |
| `totalFoldedCount`       | decimal  | Tổng số lượng gấp xúc            |
| `totalProcessingMold`    | decimal  | Tổng DC gia công (kg)            |
| `totalBlowingStageMold`  | decimal  | Tổng DC công đoạn Thổi (kg)      |
| `totalPrintingStageMold` | decimal  | Tổng DC công đoạn In (kg)        |
| `totalCuttingStageMold`  | decimal  | Tổng DC công đoạn Cắt (kg)       |
| `lines`                  | array    | Danh sách chi tiết công đoạn cắt |

#### Thông tin Chi tiết (CuttingProcessLine)

| Trường                  | Kiểu     | Mô tả                              |
| ----------------------- | -------- | ---------------------------------- |
| `id`                    | integer  | ID chi tiết công đoạn              |
| `productionOrderId`     | integer  | ID lệnh sản xuất SAP               |
| `itemCode`              | string   | Mã hàng                            |
| `productionBatch`       | string   | Lô sản xuất                        |
| `cardCode`              | string   | Mã khách hàng                      |
| `customerName`          | string   | Tên khách hàng                     |
| `productType`           | string   | Chủng loại                         |
| `productTypeName`       | string   | Tên chủng loại                     |
| `thickness`             | string   | Độ dày / 1 lá                      |
| `semiProductWidth`      | string   | Khổ màng BTP                       |
| `size`                  | string   | Kích thước                         |
| `colorCount`            | string   | Số màu in                          |
| `cuttingMachine`        | string   | Máy cắt                            |
| `workerId`              | integer  | ID công nhân cắt                   |
| `workerName`            | string   | Tên công nhân cắt                  |
| `cuttingSpeed`          | decimal  | Tốc độ cắt                         |
| `startTime`             | datetime | Thời gian bắt đầu cắt              |
| `endTime`               | datetime | Thời gian kết thúc cắt             |
| `machineStopMinutes`    | decimal  | Thời gian dừng máy (phút)          |
| `stopReason`            | string   | Nguyên nhân dừng máy               |
| `pieceCount`            | decimal  | Số chiếc (sản lượng cắt)           |
| `QuantityKg`            | decimal  | Số kg (sản lượng cắt)              |
| `bagCount`              | decimal  | Số bao (sản lượng cắt)             |
| `foldedCount`           | decimal  | Số lượng gấp xúc                   |
| `requiredDate`          | datetime | Ngày cần hàng                      |
| `isCompleted`           | boolean  | Xác nhận hoàn thành                |
| `actualCompletionDate`  | datetime | Ngày hoàn thành thực tế            |
| `delayReason`           | string   | Nguyên nhân chậm tiến độ           |
| `processingLossKg`      | decimal  | DC gia công (kg)                   |
| `processingLossReason`  | string   | DC gia công - nguyên nhân          |
| `blowingLossKg`         | decimal  | DC do công đoạn thổi (kg)          |
| `blowingLossReason`     | string   | DC do công đoạn thổi - nguyên nhân |
| `printingLossKg`        | decimal  | DC do công đoạn in (kg)            |
| `printingLossReason`    | string   | DC do công đoạn in - nguyên nhân   |
| `printingMachine`       | string   | Số máy in                          |
| `transferKg`            | decimal  | Chuyển hàng (kg)                   |
| `humanLossKg`           | decimal  | DC do cắt dán - Con người (kg)     |
| `humanLossReason`       | string   | DC - Nguyên nhân con người         |
| `machineLossKg`         | decimal  | DC do cắt dán - Lỗi máy (kg)       |
| `machineLossReason`     | string   | DC - Nguyên nhân lỗi máy           |
| `totalLossKg`           | decimal  | Tổng DC (kg)                       |
| `excessPOLess5Kg`       | decimal  | Thừa PO - Cuộn dưới 5kg            |
| `excessPOOver5Kg`       | decimal  | Thừa PO - Cuộn trên 5kg            |
| `excessPOCut`           | decimal  | Thừa PO - Hàng cắt (kg)            |
| `btpWarehouseConfirmed` | boolean  | Xác nhận của kho BTP               |
| `remainingInventoryKg`  | decimal  | Tồn (kg)                           |

### Response Error (404 Not Found)

```json
{
  "message": "Không tìm thấy công đoạn cắt",
  "statusCode": 404
}
```

---

## 3. Tạo Công Đoạn Cắt Mới

### Endpoint

```http
POST /api/cutting-processes
```

### Mô tả

Tạo một công đoạn cắt mới.

### Request Body

```json
{
  "shiftLeaderId": 5,
  "isDraft": false,
  "productionDate": "2025-11-03T00:00:00",
  "productionShift": "Ca 1",
  "lines": [
    {
      "productionOrderId": 1,
      "size": "30x40cm",
      "colorCount": "4 màu",
      "cuttingMachine": "Máy cắt 01",
      "workerId": 10,
      "cuttingSpeed": 120.0,
      "startTime": "2025-11-03T07:00:00",
      "endTime": "2025-11-03T15:00:00",
      "machineStopMinutes": 30.0,
      "stopReason": "Thay dao cắt",
      "pieceCount": 5000.0,
      "QuantityKg": 1200.0,
      "bagCount": 100.0,
      "foldedCount": 500.0,
      "requiredDate": "2025-11-04T00:00:00",
      "isCompleted": true,
      "actualCompletionDate": "2025-11-03T15:00:00",
      "delayReason": null,
      "processingLossKg": 30.0,
      "processingLossReason": "DC gia công",
      "blowingLossKg": 15.0,
      "blowingLossReason": "DC từ công đoạn thổi",
      "printingLossKg": 10.0,
      "printingLossReason": "DC từ công đoạn in",
      "printingMachine": "Máy in 02",
      "transferKg": 50.0,
      "humanLossKg": 8.0,
      "humanLossReason": "Lỗi thao tác",
      "machineLossKg": 7.0,
      "machineLossReason": "Máy lỗi",
      "ExcessPOLess5Kg": 5.0,
      "ExcessPOOver5Kg": 10.0,
      "ExcessPOCut": 8.0,
      "btpWarehouseConfirmed": true,
      "remainingInventoryKg": 100.0
    }
  ]
}
```

### Mô tả các trường Request (CreateCuttingProcessDto)

#### Thông tin Header

| Trường            | Kiểu     | Bắt buộc | Giới hạn     | Mô tả                            |
| ----------------- | -------- | -------- | ------------ | -------------------------------- |
| `shiftLeaderId`   | integer  | **Có**   | -            | ID trưởng ca                     |
| `isDraft`         | boolean  | Không    | -            | Có phải bản nháp không           |
| `productionDate`  | datetime | **Có**   | -            | Ngày sản xuất                    |
| `productionShift` | string   | **Có**   | Max 50 ký tự | Ca sản xuất                      |
| `lines`           | array    | Không    | -            | Danh sách chi tiết công đoạn cắt |

#### Thông tin Chi tiết (CreateCuttingProcessLineDto)

| Trường                  | Kiểu     | Bắt buộc | Giới hạn      | Mô tả                              |
| ----------------------- | -------- | -------- | ------------- | ---------------------------------- |
| `productionOrderId`     | integer  | **Có**   | -             | DocEntry của lệnh sản xuất         |
| `size`                  | string   | Không    | Max 100 ký tự | Kích thước                         |
| `colorCount`            | string   | Không    | Max 50 ký tự  | Số màu in                          |
| `cuttingMachine`        | string   | Không    | Max 50 ký tự  | Máy cắt                            |
| `workerId`              | integer  | Không    | -             | ID công nhân cắt                   |
| `cuttingSpeed`          | decimal  | Không    | >= 0          | Tốc độ cắt                         |
| `startTime`             | datetime | Không    | -             | Thời gian bắt đầu cắt              |
| `endTime`               | datetime | Không    | -             | Thời gian kết thúc cắt             |
| `machineStopMinutes`    | decimal  | Không    | >= 0          | Thời gian dừng máy (phút)          |
| `stopReason`            | string   | Không    | Max 500 ký tự | Nguyên nhân dừng máy               |
| `pieceCount`            | decimal  | Không    | >= 0          | Số chiếc (sản lượng cắt)           |
| `QuantityKg`            | decimal  | Không    | >= 0          | Số kg (sản lượng cắt)              |
| `bagCount`              | decimal  | Không    | >= 0          | Số bao (sản lượng cắt)             |
| `foldedCount`           | decimal  | Không    | >= 0          | Số lượng gấp xúc                   |
| `requiredDate`          | datetime | Không    | -             | Ngày cần hàng                      |
| `isCompleted`           | boolean  | Không    | -             | Xác nhận hoàn thành                |
| `actualCompletionDate`  | datetime | Không    | -             | Ngày hoàn thành thực tế            |
| `delayReason`           | string   | Không    | Max 500 ký tự | Nguyên nhân chậm tiến độ           |
| `processingLossKg`      | decimal  | Không    | >= 0          | DC gia công (kg)                   |
| `processingLossReason`  | string   | Không    | Max 500 ký tự | DC gia công - nguyên nhân          |
| `blowingLossKg`         | decimal  | Không    | >= 0          | DC do công đoạn thổi (kg)          |
| `blowingLossReason`     | string   | Không    | Max 500 ký tự | DC do công đoạn thổi - nguyên nhân |
| `printingLossKg`        | decimal  | Không    | >= 0          | DC do công đoạn in (kg)            |
| `printingLossReason`    | string   | Không    | Max 500 ký tự | DC do công đoạn in - nguyên nhân   |
| `printingMachine`       | string   | Không    | Max 50 ký tự  | Số máy in                          |
| `transferKg`            | decimal  | Không    | >= 0          | Chuyển hàng (kg)                   |
| `humanLossKg`           | decimal  | Không    | >= 0          | DC do cắt dán - Con người (kg)     |
| `humanLossReason`       | string   | Không    | Max 500 ký tự | DC - Nguyên nhân con người         |
| `machineLossKg`         | decimal  | Không    | >= 0          | DC do cắt dán - Lỗi máy (kg)       |
| `machineLossReason`     | string   | Không    | Max 500 ký tự | DC - Nguyên nhân lỗi máy           |
| `excessPOLess5Kg`       | decimal  | Không    | >= 0          | Thừa PO - Cuộn dưới 5kg            |
| `excessPOOver5Kg`       | decimal  | Không    | >= 0          | Thừa PO - Cuộn trên 5kg            |
| `excessPOCut`           | decimal  | Không    | >= 0          | Thừa PO - Hàng cắt (kg)            |
| `btpWarehouseConfirmed` | boolean  | Không    | -             | Xác nhận của kho BTP               |
| `remainingInventoryKg`  | decimal  | Không    | >= 0          | Tồn (kg)                           |

### Ví dụ Request

```http
POST /api/cutting-processes
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
Content-Type: application/json

{
  "shiftLeaderId": 5,
  "productionDate": "2025-11-03T00:00:00",
  "productionShift": "Ca 1",
  "lines": [
    {
      "productionOrderId": 1,
      "QuantityKg": 1200.0
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

## 4. Cập Nhật Công Đoạn Cắt

### Endpoint

```http
PUT /api/cutting-processes/{id}
```

### Mô tả

Cập nhật thông tin công đoạn cắt theo ID.

### Path Parameters

| Tên  | Kiểu    | Bắt buộc | Mô tả                         |
| ---- | ------- | -------- | ----------------------------- |
| `id` | integer | Có       | ID công đoạn cắt cần cập nhật |

### Request Body

```json
{
  "shiftLeaderId": 5,
  "isDraft": false,
  "productionDate": "2025-11-03T00:00:00",
  "productionShift": "Ca 1",
  "lines": [
    {
      "id": 1,
      "productionOrderId": 1,
      "productType": "PE",
      "thickness": "50",
      "semiProductWidth": "1200mm",
      "size": "30x40cm",
      "colorCount": "4 màu",
      "cuttingMachine": "Máy cắt 01",
      "workerId": 10,
      "cuttingSpeed": 120.0,
      "startTime": "2025-11-03T07:00:00",
      "endTime": "2025-11-03T15:00:00",
      "machineStopMinutes": 30.0,
      "stopReason": "Thay dao cắt",
      "pieceCount": 5000.0,
      "QuantityKg": 1250.0,
      "bagCount": 105.0,
      "foldedCount": 520.0,
      "requiredDate": "2025-11-04T00:00:00",
      "isCompleted": true,
      "actualCompletionDate": "2025-11-03T15:00:00",
      "delayReason": null,
      "processingLossKg": 30.0,
      "processingLossReason": "DC gia công",
      "blowingLossKg": 15.0,
      "blowingLossReason": "DC từ công đoạn thổi",
      "printingLossKg": 10.0,
      "printingLossReason": "DC từ công đoạn in",
      "printingMachine": "Máy in 02",
      "transferKg": 50.0,
      "humanLossKg": 8.0,
      "humanLossReason": "Lỗi thao tác",
      "machineLossKg": 7.0,
      "machineLossReason": "Máy lỗi",
      "ExcessPOLess5Kg": 5.0,
      "ExcessPOOver5Kg": 10.0,
      "ExcessPOCut": 8.0,
      "btpWarehouseConfirmed": true,
      "remainingInventoryKg": 100.0
    },
    {
      "id": null,
      "productionOrderId": 2,
      "QuantityKg": 800.0
    }
  ]
}
```

### Mô tả các trường Request (UpdateCuttingProcessDto)

#### Thông tin Header

| Trường            | Kiểu     | Bắt buộc | Giới hạn     | Mô tả                            |
| ----------------- | -------- | -------- | ------------ | -------------------------------- |
| `shiftLeaderId`   | integer  | **Có**   | -            | ID trưởng ca                     |
| `isDraft`         | boolean  | Không    | -            | Có phải bản nháp không           |
| `productionDate`  | datetime | **Có**   | -            | Ngày sản xuất                    |
| `productionShift` | string   | **Có**   | Max 50 ký tự | Ca sản xuất                      |
| `lines`           | array    | Không    | -            | Danh sách chi tiết công đoạn cắt |

#### Thông tin Chi tiết (UpdateCuttingProcessLineDto)

| Trường                  | Kiểu     | Bắt buộc | Giới hạn      | Mô tả                                        |
| ----------------------- | -------- | -------- | ------------- | -------------------------------------------- |
| `id`                    | integer  | Không    | -             | ID chi tiết công đoạn (null nếu là dòng mới) |
| `productionOrderId`     | integer  | **Có**   | -             | ID lệnh sản xuất SAP                         |
| `productType`           | string   | Không    | Max 100 ký tự | Chủng loại                                   |
| `thickness`             | string   | Không    | Max 50 ký tự  | Độ dày / 1 lá                                |
| `semiProductWidth`      | string   | Không    | Max 50 ký tự  | Khổ màng BTP                                 |
| `size`                  | string   | Không    | Max 100 ký tự | Kích thước                                   |
| `colorCount`            | string   | Không    | Max 50 ký tự  | Số màu in                                    |
| `cuttingMachine`        | string   | Không    | Max 50 ký tự  | Máy cắt                                      |
| `workerId`              | integer  | Không    | -             | ID công nhân cắt                             |
| `cuttingSpeed`          | decimal  | Không    | >= 0          | Tốc độ cắt                                   |
| `startTime`             | datetime | Không    | -             | Thời gian bắt đầu cắt                        |
| `endTime`               | datetime | Không    | -             | Thời gian kết thúc cắt                       |
| `machineStopMinutes`    | decimal  | Không    | >= 0          | Thời gian dừng máy (phút)                    |
| `stopReason`            | string   | Không    | Max 500 ký tự | Nguyên nhân dừng máy                         |
| `pieceCount`            | decimal  | Không    | >= 0          | Số chiếc (sản lượng cắt)                     |
| `QuantityKg`            | decimal  | Không    | >= 0          | Số kg (sản lượng cắt)                        |
| `bagCount`              | decimal  | Không    | >= 0          | Số bao (sản lượng cắt)                       |
| `foldedCount`           | decimal  | Không    | >= 0          | Số lượng gấp xúc                             |
| `requiredDate`          | datetime | Không    | -             | Ngày cần hàng                                |
| `isCompleted`           | boolean  | Không    | -             | Xác nhận hoàn thành                          |
| `actualCompletionDate`  | datetime | Không    | -             | Ngày hoàn thành thực tế                      |
| `delayReason`           | string   | Không    | Max 500 ký tự | Nguyên nhân chậm tiến độ                     |
| `processingLossKg`      | decimal  | Không    | >= 0          | DC gia công (kg)                             |
| `processingLossReason`  | string   | Không    | Max 500 ký tự | DC gia công - nguyên nhân                    |
| `blowingLossKg`         | decimal  | Không    | >= 0          | DC do công đoạn thổi (kg)                    |
| `blowingLossReason`     | string   | Không    | Max 500 ký tự | DC do công đoạn thổi - nguyên nhân           |
| `printingLossKg`        | decimal  | Không    | >= 0          | DC do công đoạn in (kg)                      |
| `printingLossReason`    | string   | Không    | Max 500 ký tự | DC do công đoạn in - nguyên nhân             |
| `printingMachine`       | string   | Không    | Max 50 ký tự  | Số máy in                                    |
| `transferKg`            | decimal  | Không    | >= 0          | Chuyển hàng (kg)                             |
| `humanLossKg`           | decimal  | Không    | >= 0          | DC do cắt dán - Con người (kg)               |
| `humanLossReason`       | string   | Không    | Max 500 ký tự | DC - Nguyên nhân con người                   |
| `machineLossKg`         | decimal  | Không    | >= 0          | DC do cắt dán - Lỗi máy (kg)                 |
| `machineLossReason`     | string   | Không    | Max 500 ký tự | DC - Nguyên nhân lỗi máy                     |
| `excessPOLess5Kg`       | decimal  | Không    | >= 0          | Thừa PO - Cuộn dưới 5kg                      |
| `excessPOOver5Kg`       | decimal  | Không    | >= 0          | Thừa PO - Cuộn trên 5kg                      |
| `excessPOCut`           | decimal  | Không    | >= 0          | Thừa PO - Hàng cắt (kg)                      |
| `btpWarehouseConfirmed` | boolean  | Không    | -             | Xác nhận của kho BTP                         |
| `remainingInventoryKg`  | decimal  | Không    | >= 0          | Tồn (kg)                                     |

**Lưu ý:**

- Nếu `id` của line là `null`, dòng đó sẽ được thêm mới
- Nếu `id` của line có giá trị, dòng đó sẽ được cập nhật
- Các dòng có trong database nhưng không có trong request sẽ bị xóa

### Ví dụ Request

```http
PUT /api/cutting-processes/1
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
Content-Type: application/json

{
  "shiftLeaderId": 5,
  "productionDate": "2025-11-03T00:00:00",
  "productionShift": "Ca 1",
  "lines": [
    {
      "id": 1,
      "productionOrderId": 1,
      "QuantityKg": 1250.0
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
  "message": "Không tìm thấy công đoạn cắt với ID: 1",
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

## 5. Xóa Công Đoạn Cắt

### Endpoint

```http
DELETE /api/cutting-processes/{id}
```

### Mô tả

Xóa một công đoạn cắt theo ID. Tất cả các chi tiết công đoạn cắt liên quan cũng sẽ bị xóa.

### Path Parameters

| Tên  | Kiểu    | Bắt buộc | Mô tả                    |
| ---- | ------- | -------- | ------------------------ |
| `id` | integer | Có       | ID công đoạn cắt cần xóa |

### Ví dụ Request

```http
DELETE /api/cutting-processes/1
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

### Response Success (204 No Content)

```
(Không có nội dung trả về)
```

### Response Error (404 Not Found)

```json
{
  "message": "Không tìm thấy công đoạn cắt với ID: 1",
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

### 1. Lấy danh sách công đoạn cắt

```bash
curl -X GET "http://localhost:5000/api/cutting-processes?page=1&pageSize=10" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"
```

### 2. Lấy chi tiết công đoạn cắt

```bash
curl -X GET "http://localhost:5000/api/cutting-processes/1" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"
```

### 3. Tạo công đoạn cắt mới

```bash
curl -X POST "http://localhost:5000/api/cutting-processes" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE" \
  -H "Content-Type: application/json" \
  -d '{
    "shiftLeaderId": 5,
    "productionDate": "2025-11-03T00:00:00",
    "productionShift": "Ca 1",
    "lines": [
      {
        "productionOrderId": 1,
        "QuantityKg": 1200.0
      }
    ]
  }'
```

### 4. Cập nhật công đoạn cắt

```bash
curl -X PUT "http://localhost:5000/api/cutting-processes/1" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE" \
  -H "Content-Type: application/json" \
  -d '{
    "shiftLeaderId": 5,
    "productionDate": "2025-11-03T00:00:00",
    "productionShift": "Ca 1",
    "lines": [
      {
        "id": 1,
        "productionOrderId": 1,
        "QuantityKg": 1250.0
      }
    ]
  }'
```

### 5. Xóa công đoạn cắt

```bash
curl -X DELETE "http://localhost:5000/api/cutting-processes/1" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"
```

---

## Ví dụ Sử dụng với JavaScript (Fetch API)

### 1. Lấy danh sách công đoạn cắt

```javascript
const token = "YOUR_TOKEN_HERE";

fetch("http://localhost:5000/api/cutting-processes?page=1&pageSize=10", {
  method: "GET",
  headers: {
    Authorization: `Bearer ${token}`,
  },
})
  .then((response) => response.json())
  .then((data) => console.log(data))
  .catch((error) => console.error("Error:", error));
```

### 2. Tạo công đoạn cắt mới

```javascript
const token = "YOUR_TOKEN_HERE";
const data = {
  shiftLeaderId: 5,
  productionDate: "2025-11-03T00:00:00",
  productionShift: "Ca 1",
  lines: [
    {
      productionOrderId: 1,
      QuantityKg: 1200.0,
    },
  ],
};

fetch("http://localhost:5000/api/cutting-processes", {
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

### 3. Cập nhật công đoạn cắt

```javascript
const token = "YOUR_TOKEN_HERE";
const data = {
  shiftLeaderId: 5,
  productionDate: "2025-11-03T00:00:00",
  productionShift: "Ca 1",
  lines: [
    {
      id: 1,
      productionOrderId: 1,
      QuantityKg: 1250.0,
    },
  ],
};

fetch("http://localhost:5000/api/cutting-processes/1", {
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

### 4. Xóa công đoạn cắt

```javascript
const token = "YOUR_TOKEN_HERE";

fetch("http://localhost:5000/api/cutting-processes/1", {
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

4. **Tính toán tự động**: Các tổng (totalCuttingOutput, totalFoldedCount, totalProcessingMold, totalLossKg) sẽ được tính toán tự động dựa trên dữ liệu chi tiết.

5. **Cập nhật Lines**:

   - Lines với `id = null` sẽ được thêm mới
   - Lines với `id` có giá trị sẽ được cập nhật
   - Lines không có trong request sẽ bị xóa

6. **Kiểu dữ liệu DateTime**: Sử dụng định dạng ISO 8601 (ví dụ: `2025-11-03T00:00:00` hoặc `2025-11-03T15:30:00Z`).

7. **Kiểu dữ liệu Decimal**: Sử dụng số thập phân với dấu chấm (.), không dùng dấu phẩy (,). Ví dụ: `1200.50` thay vì `1200,50`.

8. **DC từ các công đoạn**: API hỗ trợ ghi nhận DC từ công đoạn gia công, thổi, in và cắt dán.

9. **Thừa PO**: API phân loại thừa PO theo trọng lượng cuộn (dưới 5kg, trên 5kg) và hàng cắt.

---

## Thuật Ngữ & Viết Tắt

| Thuật ngữ | Ý nghĩa                                 |
| --------- | --------------------------------------- |
| **DC**    | Defective/Damage - Sản phẩm lỗi/hư hỏng |
| **BTP**   | Bán Thành Phẩm                          |
| **PO**    | Production Order - Đơn hàng sản xuất    |
| **PE**    | Polyethylene - Loại nhựa                |

---

**Phiên bản tài liệu:** 1.0  
**Ngày cập nhật:** 03/11/2025
