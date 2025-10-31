# API Công Đoạn Thổi (Blowing Process API)

API này cung cấp các endpoint để quản lý công đoạn thổi trong quy trình sản xuất.

## Base URL
```
/api/blowing-processes
```

## Authentication
Tất cả các endpoint yêu cầu xác thực bằng JWT token. Thêm token vào header:
```
Authorization: Bearer {your_token}
```

---

## 1. Lấy Danh Sách Công Đoạn Thổi

### Endpoint
```http
GET /api/blowing-processes
```

### Mô tả
Lấy danh sách tất cả công đoạn thổi với khả năng phân trang, lọc và sắp xếp.

### Query Parameters (Gridify)

| Tên | Kiểu | Mô tả | Ví dụ |
|-----|------|-------|-------|
| `page` | integer | Số trang (mặc định: 1) | `?page=1` |
| `pageSize` | integer | Số bản ghi trên mỗi trang (mặc định: 10) | `?pageSize=20` |
| `filter` | string | Điều kiện lọc theo cú pháp Gridify | `?filter=ProductionShift=="Ca 1"` |
| `orderBy` | string | Sắp xếp theo trường | `?orderBy=ProductionDate desc` |

### Ví dụ Request
```http
GET /api/blowing-processes?page=1&pageSize=10&filter=ProductionShift=="Ca 1"&orderBy=ProductionDate desc
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
      "productionDate": "2025-10-29T00:00:00",
      "productionShift": "Ca 1",
      "totalBlowingOutput": 1500.50,
      "totalRewindingOutput": 200.30,
      "totalReservedOutput": 100.00,
      "totalBlowingLoss": 50.20,
      "lines": [
        {
          "id": 1,
          "itemCode": "PROD001",
          "productionBatch": "LOT-2025-001",
          "customerName": "Công ty ABC",
          "productType": "PE",
          "thickness": "50 micron",
          "semiProductWidth": "1200mm",
          "blowingMachine": "Máy 01",
          "workerId": 10,
          "workerName": "Trần Văn B",
          "blowingSpeed": "150 kg/h",
          "startTime": "2025-10-29T07:00:00",
          "endTime": "2025-10-29T15:00:00",
          "stopDurationMinutes": 30,
          "stopReason": "Thay nguyên liệu",
          "quantityRolls": 10.00,
          "quantityKg": 800.50,
          "rewindOrSplitWeight": 100.00,
          "reservedWeight": 50.00,
          "weighingDate": "2025-10-30T00:00:00",
          "isCompleted": true,
          "actualCompletionDate": "2025-10-29T15:00:00",
          "delayReason": null,
          "widthChange": 5.00,
          "innerCoating": 3.00,
          "trimmedEdge": 2.00,
          "electricalIssue": 0.00,
          "materialLossKg": 10.00,
          "materialLossReason": "Nguyên liệu lỗi",
          "humanErrorKg": 5.00,
          "humanErrorReason": "Nhầm lẫn cài đặt",
          "machineErrorKg": 3.00,
          "machineErrorReason": "Máy bị kẹt",
          "otherErrorKg": 2.00,
          "otherErrorReason": "Lý do khác",
          "totalLoss": 30.00,
          "excessPO": 10.00,
          "semiProductWarehouseConfirmed": false,
          "note": "Ghi chú sản xuất",
          "blowingStageInventory": 500.00
        }
      ]
    }
  ],
  "totalCount": 50,
  "page": 1,
  "pageSize": 10,
  "totalPages": 5
}
```

### Mô tả các trường Response

#### Thông tin Header (BlowingProcess)
| Trường | Kiểu | Mô tả |
|--------|------|-------|
| `id` | integer | ID công đoạn thổi |
| `shiftLeaderId` | integer | ID trưởng ca |
| `shiftLeaderName` | string | Tên trưởng ca |
| `productionDate` | datetime | Ngày sản xuất |
| `productionShift` | string | Ca sản xuất |
| `totalBlowingOutput` | decimal | Tổng sản lượng thổi (kg) |
| `totalRewindingOutput` | decimal | Tổng sản lượng tua/chia/tờ (kg) |
| `totalReservedOutput` | decimal | Tổng sản lượng dự trữ (kg) |
| `totalBlowingLoss` | decimal | Tổng DC công đoạn thổi (kg) |
| `lines` | array | Danh sách chi tiết công đoạn thổi |

#### Thông tin Chi tiết (BlowingProcessLine)
| Trường | Kiểu | Mô tả |
|--------|------|-------|
| `id` | integer | ID chi tiết công đoạn |
| `itemCode` | string | Mã hàng |
| `productionBatch` | string | Lô sản xuất |
| `customerName` | string | Khách hàng |
| `productType` | string | Chủng loại |
| `thickness` | string | Độ dày / 1 lá |
| `semiProductWidth` | string | Khổ màng BTP |
| `blowingMachine` | string | Máy thổi |
| `workerId` | integer | ID công nhân thổi |
| `workerName` | string | Tên công nhân thổi |
| `blowingSpeed` | string | Tốc độ thổi (kg/giờ) |
| `startTime` | datetime | Thời gian bắt đầu thổi |
| `endTime` | datetime | Thời gian kết thúc thổi |
| `stopDurationMinutes` | integer | Thời gian dừng máy (phút) |
| `stopReason` | string | Nguyên nhân dừng máy |
| `quantityRolls` | decimal | Sản lượng thổi (số cuộn) |
| `quantityKg` | decimal | Sản lượng thổi (số kg) |
| `rewindOrSplitWeight` | decimal | Sản lượng tua/chia/tờ (kg) |
| `reservedWeight` | decimal | Sản lượng dự trữ (kg) |
| `weighingDate` | datetime | Ngày cần hàng |
| `isCompleted` | boolean | Xác nhận hoàn thành |
| `actualCompletionDate` | datetime | Ngày hoàn thành thực tế |
| `delayReason` | string | Nguyên nhân chậm tiến độ (QLSX ghi) |
| `widthChange` | decimal | Đổi khổ (kg) |
| `innerCoating` | decimal | Tráng lòng (kg) |
| `trimmedEdge` | decimal | Cắt via (kg) |
| `electricalIssue` | decimal | Sự cố điện (kg) |
| `materialLossKg` | decimal | DC nguyên liệu (kg) |
| `materialLossReason` | string | Ghi rõ nguyên nhân DC nguyên liệu |
| `humanErrorKg` | decimal | DC con người (kg) |
| `humanErrorReason` | string | Ghi rõ nguyên nhân DC con người |
| `machineErrorKg` | decimal | DC lỗi máy (kg) |
| `machineErrorReason` | string | Ghi rõ nguyên nhân DC lỗi máy |
| `otherErrorKg` | decimal | DC lỗi khác (kg) |
| `otherErrorReason` | string | Ghi rõ nguyên nhân DC lỗi khác |
| `totalLoss` | decimal | Tổng DC (kg) |
| `excessPO` | decimal | Thừa PO (kg) |
| `semiProductWarehouseConfirmed` | boolean | Xác nhận của kho BTP |
| `note` | string | Ghi chú |
| `blowingStageInventory` | decimal | Tồn kho công đoạn Thổi (kg) |

---

## 2. Lấy Chi Tiết Công Đoạn Thổi

### Endpoint
```http
GET /api/blowing-processes/{id}
```

### Mô tả
Lấy thông tin chi tiết một công đoạn thổi theo ID.

### Path Parameters
| Tên | Kiểu | Bắt buộc | Mô tả |
|-----|------|----------|-------|
| `id` | integer | Có | ID công đoạn thổi |

### Ví dụ Request
```http
GET /api/blowing-processes/1
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

### Response Success (200 OK)
```json
{
  "id": 1,
  "shiftLeaderId": 5,
  "shiftLeaderName": "Nguyễn Văn A",
  "productionDate": "2025-10-29T00:00:00",
  "productionShift": "Ca 1",
  "totalBlowingOutput": 1500.50,
  "totalRewindingOutput": 200.30,
  "totalReservedOutput": 100.00,
  "totalBlowingLoss": 50.20,
  "lines": [
    {
      "id": 1,
      "itemCode": "PROD001",
      "productionBatch": "LOT-2025-001",
      "customerName": "Công ty ABC",
      "productType": "PE",
      "thickness": "50 micron",
      "semiProductWidth": "1200mm",
      "blowingMachine": "Máy 01",
      "workerId": 10,
      "workerName": "Trần Văn B",
      "blowingSpeed": "150 kg/h",
      "startTime": "2025-10-29T07:00:00",
      "endTime": "2025-10-29T15:00:00",
      "stopDurationMinutes": 30,
      "stopReason": "Thay nguyên liệu",
      "quantityRolls": 10.00,
      "quantityKg": 800.50,
      "rewindOrSplitWeight": 100.00,
      "reservedWeight": 50.00,
      "weighingDate": "2025-10-30T00:00:00",
      "isCompleted": true,
      "actualCompletionDate": "2025-10-29T15:00:00",
      "delayReason": null,
      "widthChange": 5.00,
      "innerCoating": 3.00,
      "trimmedEdge": 2.00,
      "electricalIssue": 0.00,
      "materialLossKg": 10.00,
      "materialLossReason": "Nguyên liệu lỗi",
      "humanErrorKg": 5.00,
      "humanErrorReason": "Nhầm lẫn cài đặt",
      "machineErrorKg": 3.00,
      "machineErrorReason": "Máy bị kẹt",
      "otherErrorKg": 2.00,
      "otherErrorReason": "Lý do khác",
      "totalLoss": 30.00,
      "excessPO": 10.00,
      "semiProductWarehouseConfirmed": false,
      "note": "Ghi chú sản xuất",
      "blowingStageInventory": 500.00
    }
  ]
}
```

### Response Error (404 Not Found)
```json
{
  "message": "Không tìm thấy công đoạn thổi",
  "statusCode": 404
}
```

---

## 3. Tạo Công Đoạn Thổi Mới

### Endpoint
```http
POST /api/blowing-processes
```

### Mô tả
Tạo một công đoạn thổi mới.

### Request Body
```json
{
  "shiftLeaderId": 5,
  "productionDate": "2025-10-29T00:00:00",
  "productionShift": "Ca 1",
  "lines": [
    {
      "itemCode": "PROD001",
      "productionBatch": "LOT-2025-001",
      "customerName": "Công ty ABC",
      "productType": "PE",
      "thickness": "50 micron",
      "semiProductWidth": "1200mm",
      "blowingMachine": "Máy 01",
      "workerId": 10,
      "blowingSpeed": "150 kg/h",
      "startTime": "2025-10-29T07:00:00",
      "endTime": "2025-10-29T15:00:00",
      "stopDurationMinutes": 30,
      "stopReason": "Thay nguyên liệu",
      "quantityRolls": 10.00,
      "quantityKg": 800.50,
      "rewindOrSplitWeight": 100.00,
      "reservedWeight": 50.00,
      "weighingDate": "2025-10-30T00:00:00",
      "isCompleted": true,
      "actualCompletionDate": "2025-10-29T15:00:00",
      "delayReason": null,
      "widthChange": 5.00,
      "innerCoating": 3.00,
      "trimmedEdge": 2.00,
      "electricalIssue": 0.00,
      "materialLossKg": 10.00,
      "materialLossReason": "Nguyên liệu lỗi",
      "humanErrorKg": 5.00,
      "humanErrorReason": "Nhầm lẫn cài đặt",
      "machineErrorKg": 3.00,
      "machineErrorReason": "Máy bị kẹt",
      "otherErrorKg": 2.00,
      "otherErrorReason": "Lý do khác",
      "excessPO": 10.00,
      "semiProductWarehouseConfirmed": false,
      "note": "Ghi chú sản xuất",
      "blowingStageInventory": 500.00
    }
  ]
}
```

### Mô tả các trường Request (CreateBlowingProcessDto)

#### Thông tin Header
| Trường | Kiểu | Bắt buộc | Giới hạn | Mô tả |
|--------|------|----------|----------|-------|
| `shiftLeaderId` | integer | **Có** | - | ID trưởng ca |
| `productionDate` | datetime | **Có** | - | Ngày sản xuất |
| `productionShift` | string | **Có** | Max 50 ký tự | Ca sản xuất |
| `lines` | array | Không | - | Danh sách chi tiết công đoạn thổi |

#### Thông tin Chi tiết (CreateBlowingProcessLineDto)
| Trường | Kiểu | Bắt buộc | Giới hạn | Mô tả |
|--------|------|----------|----------|-------|
| `itemCode` | string | **Có** | Max 50 ký tự | Mã hàng |
  `productionOrderId` |int| | **Có** | - | | DocEntry của lệch sản xuất |
| `blowingMachine` | string | Không | Max 50 ký tự | Máy thổi |
| `workerId` | integer | Không | - | ID công nhân thổi |
| `blowingSpeed` | string | Không | Max 50 ký tự | Tốc độ thổi (kg/giờ) |
| `startTime` | datetime | Không | - | Thời gian bắt đầu thổi |
| `endTime` | datetime | Không | - | Thời gian kết thúc thổi |
| `stopDurationMinutes` | integer | Không | >= 0 | Thời gian dừng máy (phút) |
| `stopReason` | string | Không | Max 500 ký tự | Nguyên nhân dừng máy |
| `quantityRolls` | decimal | Không | >= 0 | Sản lượng thổi (số cuộn) |
| `quantityKg` | decimal | Không | >= 0 | Sản lượng thổi (số kg) |
| `rewindOrSplitWeight` | decimal | Không | >= 0 | Sản lượng tua/chia/tờ (kg) |
| `reservedWeight` | decimal | Không | >= 0 | Sản lượng dự trữ (kg) |
| `weighingDate` | datetime | Không | - | Ngày cần hàng |
| `isCompleted` | boolean | Không | - | Xác nhận hoàn thành |
| `actualCompletionDate` | datetime | Không | - | Ngày hoàn thành thực tế |
| `delayReason` | string | Không | Max 500 ký tự | Nguyên nhân chậm tiến độ (QLSX ghi) |
| `widthChange` | decimal | Không | >= 0 | Đổi khổ (kg) |
| `innerCoating` | decimal | Không | >= 0 | Tráng lòng (kg) |
| `trimmedEdge` | decimal | Không | >= 0 | Cắt via (kg) |
| `electricalIssue` | decimal | Không | >= 0 | Sự cố điện (kg) |
| `materialLossKg` | decimal | Không | >= 0 | DC nguyên liệu (kg) |
| `materialLossReason` | string | Không | Max 500 ký tự | Ghi rõ nguyên nhân DC nguyên liệu |
| `humanErrorKg` | decimal | Không | >= 0 | DC con người (kg) |
| `humanErrorReason` | string | Không | Max 500 ký tự | Ghi rõ nguyên nhân DC con người |
| `machineErrorKg` | decimal | Không | >= 0 | DC lỗi máy (kg) |
| `machineErrorReason` | string | Không | Max 500 ký tự | Ghi rõ nguyên nhân DC lỗi máy |
| `otherErrorKg` | decimal | Không | >= 0 | DC lỗi khác (kg) |
| `otherErrorReason` | string | Không | Max 500 ký tự | Ghi rõ nguyên nhân DC lỗi khác |
| `excessPO` | decimal | Không | >= 0 | Thừa PO (kg) |
| `semiProductWarehouseConfirmed` | boolean | Không | - | Xác nhận của kho BTP |
| `note` | string | Không | Max 1000 ký tự | Ghi chú |
| `blowingStageInventory` | decimal | Không | >= 0 | Tồn kho công đoạn Thổi (kg) |

### Ví dụ Request
```http
POST /api/blowing-processes
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
Content-Type: application/json

{
  "shiftLeaderId": 5,
  "productionDate": "2025-10-29T00:00:00",
  "productionShift": "Ca 1",
  "lines": [
    {
      "productionOrderId": 1,
      "itemCode": "PROD001",
      "productionBatch": "LOT-2025-001",
      "customerName": "Công ty ABC",
      "quantityKg": 800.50
    }
  ]
}
```

### Response Success (200 OK)
```json
{
  "id": 1,
  "shiftLeaderId": 5,
  "shiftLeaderName": "Nguyễn Văn A",
  "productionDate": "2025-10-29T00:00:00",
  "productionShift": "Ca 1",
  "totalBlowingOutput": 800.50,
  "totalRewindingOutput": 0.00,
  "totalReservedOutput": 0.00,
  "totalBlowingLoss": 0.00,
  "lines": [
    {
      "id": 1,
      "itemCode": "PROD001",
      "productionBatch": "LOT-2025-001",
      "customerName": "Công ty ABC",
      "productType": null,
      "thickness": null,
      "semiProductWidth": null,
      "blowingMachine": null,
      "workerId": null,
      "workerName": null,
      "blowingSpeed": null,
      "startTime": null,
      "endTime": null,
      "stopDurationMinutes": 0,
      "stopReason": null,
      "quantityRolls": 0.00,
      "quantityKg": 800.50,
      "rewindOrSplitWeight": 0.00,
      "reservedWeight": 0.00,
      "weighingDate": null,
      "isCompleted": false,
      "actualCompletionDate": null,
      "delayReason": null,
      "widthChange": 0.00,
      "innerCoating": 0.00,
      "trimmedEdge": 0.00,
      "electricalIssue": 0.00,
      "materialLossKg": 0.00,
      "materialLossReason": null,
      "humanErrorKg": 0.00,
      "humanErrorReason": null,
      "machineErrorKg": 0.00,
      "machineErrorReason": null,
      "otherErrorKg": 0.00,
      "otherErrorReason": null,
      "totalLoss": 0.00,
      "excessPO": 0.00,
      "semiProductWarehouseConfirmed": false,
      "note": null,
      "blowingStageInventory": 0.00
    }
  ]
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

## 4. Cập Nhật Công Đoạn Thổi

### Endpoint
```http
PUT /api/blowing-processes/{id}
```

### Mô tả
Cập nhật thông tin công đoạn thổi theo ID.

### Path Parameters
| Tên | Kiểu | Bắt buộc | Mô tả |
|-----|------|----------|-------|
| `id` | integer | Có | ID công đoạn thổi cần cập nhật |

### Request Body
```json
{
  "shiftLeaderId": 5,
  "productionDate": "2025-10-29T00:00:00",
  "productionShift": "Ca 1",
  "lines": [
    {
      "id": 1,
      "itemCode": "PROD001",
      "productionBatch": "LOT-2025-001",
      "customerName": "Công ty ABC",
      "productType": "PE",
      "thickness": "50 micron",
      "semiProductWidth": "1200mm",
      "blowingMachine": "Máy 01",
      "workerId": 10,
      "blowingSpeed": "150 kg/h",
      "startTime": "2025-10-29T07:00:00",
      "endTime": "2025-10-29T15:00:00",
      "stopDurationMinutes": 30,
      "stopReason": "Thay nguyên liệu",
      "quantityRolls": 10.00,
      "quantityKg": 800.50,
      "rewindOrSplitWeight": 100.00,
      "reservedWeight": 50.00,
      "weighingDate": "2025-10-30T00:00:00",
      "isCompleted": true,
      "actualCompletionDate": "2025-10-29T15:00:00",
      "delayReason": null,
      "widthChange": 5.00,
      "innerCoating": 3.00,
      "trimmedEdge": 2.00,
      "electricalIssue": 0.00,
      "materialLossKg": 10.00,
      "materialLossReason": "Nguyên liệu lỗi",
      "humanErrorKg": 5.00,
      "humanErrorReason": "Nhầm lẫn cài đặt",
      "machineErrorKg": 3.00,
      "machineErrorReason": "Máy bị kẹt",
      "otherErrorKg": 2.00,
      "otherErrorReason": "Lý do khác",
      "excessPO": 10.00,
      "semiProductWarehouseConfirmed": false,
      "note": "Ghi chú sản xuất đã cập nhật",
      "blowingStageInventory": 500.00
    },
    {
      "id": null,
      "itemCode": "PROD002",
      "productionBatch": "LOT-2025-002",
      "customerName": "Công ty XYZ",
      "quantityKg": 500.00
    }
  ]
}
```

### Mô tả các trường Request (UpdateBlowingProcessDto)

#### Thông tin Header
| Trường | Kiểu | Bắt buộc | Giới hạn | Mô tả |
|--------|------|----------|----------|-------|
| `shiftLeaderId` | integer | **Có** | - | ID trưởng ca |
| `productionDate` | datetime | **Có** | - | Ngày sản xuất |
| `productionShift` | string | **Có** | Max 50 ký tự | Ca sản xuất |
| `lines` | array | Không | - | Danh sách chi tiết công đoạn thổi |

#### Thông tin Chi tiết (UpdateBlowingProcessLineDto)
| Trường | Kiểu | Bắt buộc | Giới hạn | Mô tả |
|--------|------|----------|----------|-------|
| `id` | integer | Không | - | ID chi tiết công đoạn (null nếu là dòng mới) |
| `itemCode` | string | **Có** | Max 50 ký tự | Mã hàng |
| `productionBatch` | string | **Có** | Max 100 ký tự | Lô sản xuất |
| `customerName` | string | **Có** | Max 200 ký tự | Khách hàng |
| `productType` | string | Không | Max 100 ký tự | Chủng loại |
| `thickness` | string | Không | Max 50 ký tự | Độ dày / 1 lá |
| `semiProductWidth` | string | Không | Max 50 ký tự | Khổ màng BTP |
| `blowingMachine` | string | Không | Max 50 ký tự | Máy thổi |
| `workerId` | integer | Không | - | ID công nhân thổi |
| `blowingSpeed` | string | Không | Max 50 ký tự | Tốc độ thổi (kg/giờ) |
| `startTime` | datetime | Không | - | Thời gian bắt đầu thổi |
| `endTime` | datetime | Không | - | Thời gian kết thúc thổi |
| `stopDurationMinutes` | integer | Không | >= 0 | Thời gian dừng máy (phút) |
| `stopReason` | string | Không | Max 500 ký tự | Nguyên nhân dừng máy |
| `quantityRolls` | decimal | Không | >= 0 | Sản lượng thổi (số cuộn) |
| `quantityKg` | decimal | Không | >= 0 | Sản lượng thổi (số kg) |
| `rewindOrSplitWeight` | decimal | Không | >= 0 | Sản lượng tua/chia/tờ (kg) |
| `reservedWeight` | decimal | Không | >= 0 | Sản lượng dự trữ (kg) |
| `weighingDate` | datetime | Không | - | Ngày cần hàng |
| `isCompleted` | boolean | Không | - | Xác nhận hoàn thành |
| `actualCompletionDate` | datetime | Không | - | Ngày hoàn thành thực tế |
| `delayReason` | string | Không | Max 500 ký tự | Nguyên nhân chậm tiến độ (QLSX ghi) |
| `widthChange` | decimal | Không | >= 0 | Đổi khổ (kg) |
| `innerCoating` | decimal | Không | >= 0 | Tráng lòng (kg) |
| `trimmedEdge` | decimal | Không | >= 0 | Cắt via (kg) |
| `electricalIssue` | decimal | Không | >= 0 | Sự cố điện (kg) |
| `materialLossKg` | decimal | Không | >= 0 | DC nguyên liệu (kg) |
| `materialLossReason` | string | Không | Max 500 ký tự | Ghi rõ nguyên nhân DC nguyên liệu |
| `humanErrorKg` | decimal | Không | >= 0 | DC con người (kg) |
| `humanErrorReason` | string | Không | Max 500 ký tự | Ghi rõ nguyên nhân DC con người |
| `machineErrorKg` | decimal | Không | >= 0 | DC lỗi máy (kg) |
| `machineErrorReason` | string | Không | Max 500 ký tự | Ghi rõ nguyên nhân DC lỗi máy |
| `otherErrorKg` | decimal | Không | >= 0 | DC lỗi khác (kg) |
| `otherErrorReason` | string | Không | Max 500 ký tự | Ghi rõ nguyên nhân DC lỗi khác |
| `excessPO` | decimal | Không | >= 0 | Thừa PO (kg) |
| `semiProductWarehouseConfirmed` | boolean | Không | - | Xác nhận của kho BTP |
| `note` | string | Không | Max 1000 ký tự | Ghi chú |
| `blowingStageInventory` | decimal | Không | >= 0 | Tồn kho công đoạn Thổi (kg) |

**Lưu ý:** 
- Nếu `id` của line là `null`, dòng đó sẽ được thêm mới
- Nếu `id` của line có giá trị, dòng đó sẽ được cập nhật
- Các dòng có trong database nhưng không có trong request sẽ bị xóa

### Ví dụ Request
```http
PUT /api/blowing-processes/1
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
Content-Type: application/json

{
  "shiftLeaderId": 5,
  "productionDate": "2025-10-29T00:00:00",
  "productionShift": "Ca 1",
  "lines": [
    {
      "id": 1,
      "itemCode": "PROD001",
      "productionBatch": "LOT-2025-001",
      "customerName": "Công ty ABC",
      "quantityKg": 850.50
    }
  ]
}
```

### Response Success (200 OK)
```json
{
  "id": 1,
  "shiftLeaderId": 5,
  "shiftLeaderName": "Nguyễn Văn A",
  "productionDate": "2025-10-29T00:00:00",
  "productionShift": "Ca 1",
  "totalBlowingOutput": 850.50,
  "totalRewindingOutput": 0.00,
  "totalReservedOutput": 0.00,
  "totalBlowingLoss": 0.00,
  "lines": [
    {
      "id": 1,
      "itemCode": "PROD001",
      "productionBatch": "LOT-2025-001",
      "customerName": "Công ty ABC",
      "productType": null,
      "thickness": null,
      "semiProductWidth": null,
      "blowingMachine": null,
      "workerId": null,
      "workerName": null,
      "blowingSpeed": null,
      "startTime": null,
      "endTime": null,
      "stopDurationMinutes": 0,
      "stopReason": null,
      "quantityRolls": 0.00,
      "quantityKg": 850.50,
      "rewindOrSplitWeight": 0.00,
      "reservedWeight": 0.00,
      "weighingDate": null,
      "isCompleted": false,
      "actualCompletionDate": null,
      "delayReason": null,
      "widthChange": 0.00,
      "innerCoating": 0.00,
      "trimmedEdge": 0.00,
      "electricalIssue": 0.00,
      "materialLossKg": 0.00,
      "materialLossReason": null,
      "humanErrorKg": 0.00,
      "humanErrorReason": null,
      "machineErrorKg": 0.00,
      "machineErrorReason": null,
      "otherErrorKg": 0.00,
      "otherErrorReason": null,
      "totalLoss": 0.00,
      "excessPO": 0.00,
      "semiProductWarehouseConfirmed": false,
      "note": null,
      "blowingStageInventory": 0.00
    }
  ]
}
```

### Response Error (404 Not Found)
```json
{
  "message": "Không tìm thấy công đoạn thổi với ID: 1",
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
    "Lines[0].ItemCode": ["Mã hàng là bắt buộc"]
  }
}
```

---

## 5. Xóa Công Đoạn Thổi

### Endpoint
```http
DELETE /api/blowing-processes/{id}
```

### Mô tả
Xóa một công đoạn thổi theo ID. Tất cả các chi tiết công đoạn thổi liên quan cũng sẽ bị xóa.

### Path Parameters
| Tên | Kiểu | Bắt buộc | Mô tả |
|-----|------|----------|-------|
| `id` | integer | Có | ID công đoạn thổi cần xóa |

### Ví dụ Request
```http
DELETE /api/blowing-processes/1
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

### Response Success (204 No Content)
```
(Không có nội dung trả về)
```

### Response Error (404 Not Found)
```json
{
  "message": "Không tìm thấy công đoạn thổi với ID: 1",
  "statusCode": 404
}
```

---

## Mã Lỗi Chung

| Mã lỗi | Ý nghĩa |
|--------|---------|
| 200 | Thành công |
| 204 | Thành công, không có nội dung trả về |
| 400 | Yêu cầu không hợp lệ (Bad Request) |
| 401 | Chưa xác thực (Unauthorized) |
| 403 | Không có quyền truy cập (Forbidden) |
| 404 | Không tìm thấy tài nguyên (Not Found) |
| 409 | Xung đột dữ liệu (Conflict) |
| 500 | Lỗi máy chủ nội bộ (Internal Server Error) |

---

## Ví dụ Sử dụng với cURL

### 1. Lấy danh sách công đoạn thổi
```bash
curl -X GET "http://localhost:5000/api/blowing-processes?page=1&pageSize=10" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"
```

### 2. Lấy chi tiết công đoạn thổi
```bash
curl -X GET "http://localhost:5000/api/blowing-processes/1" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"
```

### 3. Tạo công đoạn thổi mới
```bash
curl -X POST "http://localhost:5000/api/blowing-processes" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE" \
  -H "Content-Type: application/json" \
  -d '{
    "shiftLeaderId": 5,
    "productionDate": "2025-10-29T00:00:00",
    "productionShift": "Ca 1",
    "lines": [
      {
        "itemCode": "PROD001",
        "productionBatch": "LOT-2025-001",
        "customerName": "Công ty ABC",
        "quantityKg": 800.50
      }
    ]
  }'
```

### 4. Cập nhật công đoạn thổi
```bash
curl -X PUT "http://localhost:5000/api/blowing-processes/1" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE" \
  -H "Content-Type: application/json" \
  -d '{
    "shiftLeaderId": 5,
    "productionDate": "2025-10-29T00:00:00",
    "productionShift": "Ca 1",
    "lines": [
      {
        "id": 1,
        "itemCode": "PROD001",
        "productionBatch": "LOT-2025-001",
        "customerName": "Công ty ABC",
        "quantityKg": 850.50
      }
    ]
  }'
```

### 5. Xóa công đoạn thổi
```bash
curl -X DELETE "http://localhost:5000/api/blowing-processes/1" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"
```

---

## Ví dụ Sử dụng với JavaScript (Fetch API)

### 1. Lấy danh sách công đoạn thổi
```javascript
const token = 'YOUR_TOKEN_HERE';

fetch('http://localhost:5000/api/blowing-processes?page=1&pageSize=10', {
  method: 'GET',
  headers: {
    'Authorization': `Bearer ${token}`
  }
})
.then(response => response.json())
.then(data => console.log(data))
.catch(error => console.error('Error:', error));
```

### 2. Tạo công đoạn thổi mới
```javascript
const token = 'YOUR_TOKEN_HERE';
const data = {
  shiftLeaderId: 5,
  productionDate: '2025-10-29T00:00:00',
  productionShift: 'Ca 1',
  lines: [
    {
      itemCode: 'PROD001',
      productionBatch: 'LOT-2025-001',
      customerName: 'Công ty ABC',
      quantityKg: 800.50
    }
  ]
};

fetch('http://localhost:5000/api/blowing-processes', {
  method: 'POST',
  headers: {
    'Authorization': `Bearer ${token}`,
    'Content-Type': 'application/json'
  },
  body: JSON.stringify(data)
})
.then(response => response.json())
.then(data => console.log(data))
.catch(error => console.error('Error:', error));
```

### 3. Cập nhật công đoạn thổi
```javascript
const token = 'YOUR_TOKEN_HERE';
const data = {
  shiftLeaderId: 5,
  productionDate: '2025-10-29T00:00:00',
  productionShift: 'Ca 1',
  lines: [
    {
      id: 1,
      itemCode: 'PROD001',
      productionBatch: 'LOT-2025-001',
      customerName: 'Công ty ABC',
      quantityKg: 850.50
    }
  ]
};

fetch('http://localhost:5000/api/blowing-processes/1', {
  method: 'PUT',
  headers: {
    'Authorization': `Bearer ${token}`,
    'Content-Type': 'application/json'
  },
  body: JSON.stringify(data)
})
.then(response => response.json())
.then(data => console.log(data))
.catch(error => console.error('Error:', error));
```

### 4. Xóa công đoạn thổi
```javascript
const token = 'YOUR_TOKEN_HERE';

fetch('http://localhost:5000/api/blowing-processes/1', {
  method: 'DELETE',
  headers: {
    'Authorization': `Bearer ${token}`
  }
})
.then(response => {
  if (response.status === 204) {
    console.log('Xóa thành công');
  }
})
.catch(error => console.error('Error:', error));
```

---

## Lưu Ý Quan Trọng

1. **Xác thực**: Tất cả các API đều yêu cầu JWT token hợp lệ trong header Authorization.

2. **Phân trang**: API GetAll hỗ trợ phân trang với Gridify. Có thể lọc, sắp xếp theo nhiều trường khác nhau.

3. **Validation**: Các trường bắt buộc phải được cung cấp, nếu không API sẽ trả về lỗi 400 Bad Request.

4. **Tính toán tự động**: Các tổng (totalBlowingOutput, totalRewindingOutput, totalReservedOutput, totalBlowingLoss, totalLoss) sẽ được tính toán tự động dựa trên dữ liệu chi tiết.

5. **Cập nhật Lines**: 
   - Lines với `id = null` sẽ được thêm mới
   - Lines với `id` có giá trị sẽ được cập nhật
   - Lines không có trong request sẽ bị xóa

6. **Kiểu dữ liệu DateTime**: Sử dụng định dạng ISO 8601 (ví dụ: `2025-10-29T00:00:00` hoặc `2025-10-29T15:30:00Z`).

7. **Kiểu dữ liệu Decimal**: Sử dụng số thập phân với dấu chấm (.), không dùng dấu phẩy (,). Ví dụ: `800.50` thay vì `800,50`.

---

## Thuật Ngữ & Viết Tắt

| Thuật ngữ | Ý nghĩa |
|-----------|---------|
| **DC** | Defective/Damage - Sản phẩm lỗi/hư hỏng |
| **BTP** | Bán Thành Phẩm |
| **PO** | Production Order - Đơn hàng sản xuất |
| **QLSX** | Quản Lý Sản Xuất |
| **PE** | Polyethylene - Loại nhựa |

---

**Phiên bản tài liệu:** 1.0  
**Ngày cập nhật:** 29/10/2025
