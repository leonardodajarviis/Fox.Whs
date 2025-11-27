# Grain Mixing Blowing Process API

API để quản lý công đoạn pha hạt (Thổi) trong hệ thống.

## Base URL
```
/api/grain-mixing-blowing-processes
```

## Authentication
Tất cả các endpoint đều yêu cầu xác thực thông qua JWT token trong header:
```
Authorization: Bearer {token}
```

---

## Endpoints

### 1. Lấy danh sách công đoạn pha hạt (Thổi)

**GET** `/api/grain-mixing-blowing-processes`

Lấy danh sách tất cả công đoạn pha hạt (Thổi) với phân trang và lọc.

#### Query Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| page | int | No | Số trang (mặc định: 1) |
| pageSize | int | No | Kích thước trang (mặc định: 10) |
| filter | string | No | Bộ lọc Gridify (ví dụ: `ProductionDate >= 2024-01-01`) |
| orderBy | string | No | Sắp xếp (ví dụ: `ProductionDate desc`) |

#### Response
```json
{
  "results": [
    {
      "id": 1,
      "isDraft": false,
      "productionDate": "2024-01-15T00:00:00",
      "blowingMachine": "Máy thổi 01",
      "creatorId": 1,
      "modifierId": null,
      "createdAt": "2024-01-15T08:00:00",
      "modifiedAt": null,
      "creatorName": "Nguyễn Văn A",
      "modifierName": null,
      "lines": []
    }
  ],
  "totalCount": 100,
  "page": 1,
  "pageSize": 10
}
```

---

### 2. Lấy chi tiết công đoạn pha hạt (Thổi)

**GET** `/api/grain-mixing-blowing-processes/{id}`

Lấy thông tin chi tiết của một công đoạn pha hạt (Thổi) theo ID.

#### Path Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| id | int | Yes | ID của công đoạn pha hạt (Thổi) |

#### Response
```json
{
  "id": 1,
  "isDraft": false,
  "productionDate": "2024-01-15T00:00:00",
  "blowingMachine": "Máy thổi 01",
  "creatorId": 1,
  "modifierId": null,
  "createdAt": "2024-01-15T08:00:00",
  "modifiedAt": null,
  "creatorName": "Nguyễn Văn A",
  "modifierName": null,
  "lines": [
    {
      "id": 1,
      "grainMixingBlowingProcessId": 1,
      "productionBatch": "LOT001",
      "cardCode": "C00001",
      "customerName": "Công ty ABC",
      "materialIssueVoucherNo": "MIT001",
      "mixtureType": "HD",
      "specification": "HD trắng sữa 30 micron",
      "workerId": 10,
      "workerName": "Trần Văn B",
      "machineName": "Máy pha 01",
      "startTime": "2024-01-15T08:00:00",
      "endTime": "2024-01-15T16:00:00",
      "ppTron": 0,
      "ppHdNhot": 0,
      "ppLdpe": 0,
      "ppDc": 0,
      "ppAdditive": 0,
      "ppColor": 0,
      "ppOther": 0,
      "ppRit": 0,
      "hdLldpe2320": 100.5,
      "hdRecycled": 20.0,
      "hdTalcol": 5.0,
      "hdDc": 2.0,
      "hdColor": 1.5,
      "hdOther": 0,
      "hdHd": 0,
      "peAdditive": 0,
      "peTalcol": 0,
      "peColor": 0,
      "peOther": 0,
      "peLdpe": 0,
      "peLldpe": 0,
      "peRecycled": 0,
      "peDc": 0,
      "shrinkRe707": 0,
      "shrinkSlip": 0,
      "shrinkStatic": 0,
      "shrinkDc": 0,
      "shrinkTalcol": 0,
      "shrinkOther": 0,
      "shrinkLldpe": 0,
      "shrinkRecycled": 0,
      "shrinkTangDai": 0,
      "wrapRecycledCa": 0,
      "wrapRecycledCb": 0,
      "wrapGlue": 0,
      "wrapColor": 0,
      "wrapDc": 0,
      "wrapLdpe": 0,
      "wrapLldpe": 0,
      "wrapSlip": 0,
      "wrapAdditive": 0,
      "wrapOther": 0,
      "wrapTangDaiC6": 0,
      "wrapTangDaiC8": 0,
      "evaPop3070": 0,
      "evaLdpe": 0,
      "evaDc": 0,
      "evaTalcol": 0,
      "evaSlip": 0,
      "evaStaticAdditive": 0,
      "evaOther": 0,
      "evaTgc": 0,
      "quantityKg": 129.0,
      "requiredDate": "2024-01-20T00:00:00",
      "isCompleted": true,
      "status": 1,
      "actualCompletionDate": "2024-01-15T16:00:00",
      "delayReason": null
    }
  ]
}
```

#### Error Responses
- **404 Not Found**: Không tìm thấy công đoạn pha hạt (Thổi) với ID đã cho

---

### 3. Tạo công đoạn pha hạt (Thổi) mới

**POST** `/api/grain-mixing-blowing-processes`

Tạo mới một công đoạn pha hạt (Thổi).

#### Request Body
```json
{
  "isDraft": false,
  "productionDate": "2024-01-15T00:00:00",
  "blowingMachine": "Máy thổi 01",
  "lines": [
    {
      "productionBatch": "LOT001",
      "cardCode": "C00001",
      "materialIssueVoucherNo": "MIT001",
      "mixtureType": "HD",
      "specification": "HD trắng sữa 30 micron",
      "workerId": 10,
      "machineName": "Máy pha 01",
      "startTime": "2024-01-15T08:00:00",
      "endTime": "2024-01-15T16:00:00",
      "ppTron": 0,
      "ppHdNhot": 0,
      "ppLdpe": 0,
      "ppDc": 0,
      "ppAdditive": 0,
      "ppColor": 0,
      "ppOther": 0,
      "ppRit": 0,
      "hdLldpe2320": 100.5,
      "hdRecycled": 20.0,
      "hdTalcol": 5.0,
      "hdDc": 2.0,
      "hdColor": 1.5,
      "hdOther": 0,
      "hdHd": 0,
      "peAdditive": 0,
      "peTalcol": 0,
      "peColor": 0,
      "peOther": 0,
      "peLdpe": 0,
      "peLldpe": 0,
      "peRecycled": 0,
      "peDc": 0,
      "shrinkRe707": 0,
      "shrinkSlip": 0,
      "shrinkStatic": 0,
      "shrinkDc": 0,
      "shrinkTalcol": 0,
      "shrinkOther": 0,
      "shrinkLldpe": 0,
      "shrinkRecycled": 0,
      "shrinkTangDai": 0,
      "wrapRecycledCa": 0,
      "wrapRecycledCb": 0,
      "wrapGlue": 0,
      "wrapColor": 0,
      "wrapDc": 0,
      "wrapLdpe": 0,
      "wrapLldpe": 0,
      "wrapSlip": 0,
      "wrapAdditive": 0,
      "wrapOther": 0,
      "wrapTangDaiC6": 0,
      "wrapTangDaiC8": 0,
      "evaPop3070": 0,
      "evaLdpe": 0,
      "evaDc": 0,
      "evaTalcol": 0,
      "evaSlip": 0,
      "evaStaticAdditive": 0,
      "evaOther": 0,
      "evaTgc": 0,
      "quantityKg": 129.0,
      "requiredDate": "2024-01-20T00:00:00",
      "isCompleted": true,
      "status": 1,
      "actualCompletionDate": "2024-01-15T16:00:00",
      "delayReason": null
    }
  ]
}
```

#### Response
```json
{
  "id": 1,
  "isDraft": false,
  "productionDate": "2024-01-15T00:00:00",
  "blowingMachine": "Máy thổi 01",
  "creatorId": 1,
  "modifierId": null,
  "createdAt": "2024-01-15T08:00:00",
  "modifiedAt": null,
  "creatorName": "Nguyễn Văn A",
  "modifierName": null,
  "lines": [...]
}
```

#### Error Responses
- **400 Bad Request**: Dữ liệu không hợp lệ
- **404 Not Found**: Không tìm thấy công nhân hoặc khách hàng
- **401 Unauthorized**: Không xác định được người dùng hiện tại

---

### 4. Cập nhật công đoạn pha hạt (Thổi)

**PUT** `/api/grain-mixing-blowing-processes/{id}`

Cập nhật thông tin của một công đoạn pha hạt (Thổi).

#### Path Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| id | int | Yes | ID của công đoạn pha hạt (Thổi) cần cập nhật |

#### Request Body
```json
{
  "isDraft": false,
  "productionDate": "2024-01-15T00:00:00",
  "blowingMachine": "Máy thổi 02",
  "lines": [
    {
      "id": 1,
      "productionBatch": "LOT001-UPDATED",
      "cardCode": "C00001",
      "materialIssueVoucherNo": "MIT001",
      "mixtureType": "HD",
      "specification": "HD trắng sữa 30 micron - Updated",
      "workerId": 10,
      "machineName": "Máy pha 01",
      "startTime": "2024-01-15T08:00:00",
      "endTime": "2024-01-15T17:00:00",
      "ppTron": 0,
      "ppHdNhot": 0,
      "ppLdpe": 0,
      "ppDc": 0,
      "ppAdditive": 0,
      "ppColor": 0,
      "ppOther": 0,
      "ppRit": 0,
      "hdLldpe2320": 110.0,
      "hdRecycled": 22.0,
      "hdTalcol": 5.5,
      "hdDc": 2.2,
      "hdColor": 1.8,
      "hdOther": 0,
      "hdHd": 0,
      "peAdditive": 0,
      "peTalcol": 0,
      "peColor": 0,
      "peOther": 0,
      "peLdpe": 0,
      "peLldpe": 0,
      "peRecycled": 0,
      "peDc": 0,
      "shrinkRe707": 0,
      "shrinkSlip": 0,
      "shrinkStatic": 0,
      "shrinkDc": 0,
      "shrinkTalcol": 0,
      "shrinkOther": 0,
      "shrinkLldpe": 0,
      "shrinkRecycled": 0,
      "shrinkTangDai": 0,
      "wrapRecycledCa": 0,
      "wrapRecycledCb": 0,
      "wrapGlue": 0,
      "wrapColor": 0,
      "wrapDc": 0,
      "wrapLdpe": 0,
      "wrapLldpe": 0,
      "wrapSlip": 0,
      "wrapAdditive": 0,
      "wrapOther": 0,
      "wrapTangDaiC6": 0,
      "wrapTangDaiC8": 0,
      "evaPop3070": 0,
      "evaLdpe": 0,
      "evaDc": 0,
      "evaTalcol": 0,
      "evaSlip": 0,
      "evaStaticAdditive": 0,
      "evaOther": 0,
      "evaTgc": 0,
      "quantityKg": 141.5,
      "requiredDate": "2024-01-20T00:00:00",
      "isCompleted": true,
      "status": 1,
      "actualCompletionDate": "2024-01-15T17:00:00",
      "delayReason": null
    }
  ]
}
```

#### Response
```json
{
  "id": 1,
  "isDraft": false,
  "productionDate": "2024-01-15T00:00:00",
  "blowingMachine": "Máy thổi 02",
  "creatorId": 1,
  "modifierId": 1,
  "createdAt": "2024-01-15T08:00:00",
  "modifiedAt": "2024-01-16T10:30:00",
  "creatorName": "Nguyễn Văn A",
  "modifierName": "Nguyễn Văn A",
  "lines": [...]
}
```

#### Error Responses
- **400 Bad Request**: Dữ liệu không hợp lệ
- **404 Not Found**: Không tìm thấy công đoạn pha hạt (Thổi), công nhân hoặc khách hàng
- **401 Unauthorized**: Không xác định được người dùng hiện tại

---

### 5. Xóa công đoạn pha hạt (Thổi)

**DELETE** `/api/grain-mixing-blowing-processes/{id}`

Xóa một công đoạn pha hạt (Thổi).

#### Path Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| id | int | Yes | ID của công đoạn pha hạt (Thổi) cần xóa |

#### Response
- **204 No Content**: Xóa thành công

#### Error Responses
- **404 Not Found**: Không tìm thấy công đoạn pha hạt (Thổi) với ID đã cho

---

## Data Models

### GrainMixingBlowingProcess
| Field | Type | Description |
|-------|------|-------------|
| id | int | ID của công đoạn pha hạt (Thổi) |
| isDraft | bool | Bản nháp |
| productionDate | datetime | Ngày sản xuất (ngày pha) |
| blowingMachine | string? | Máy thổi |
| creatorId | short | ID người tạo |
| modifierId | short? | ID người sửa cuối |
| createdAt | datetime | Thời gian tạo |
| modifiedAt | datetime? | Thời gian sửa cuối |
| creatorName | string? | Tên người tạo |
| modifierName | string? | Tên người sửa cuối |
| lines | array | Danh sách chi tiết công đoạn pha hạt (Thổi) |

### GrainMixingBlowingProcessLine
| Field | Type | Description |
|-------|------|-------------|
| id | int | ID của chi tiết |
| grainMixingBlowingProcessId | int | ID công đoạn pha hạt (Thổi) |
| productionBatch | string? | Lô sản xuất |
| cardCode | string? | Mã khách hàng |
| customerName | string? | Tên khách hàng (từ BusinessPartner) |
| materialIssueVoucherNo | string? | Số phiếu lĩnh vật tư |
| mixtureType | string? | Chủng loại pha (HD, PE, Màng co, Màng chít, PP, EVA) |
| specification | string? | Quy cách diễn giải |
| workerId | int? | ID công nhân pha |
| workerName | string? | Tên công nhân pha |
| machineName | string? | Tên máy pha |
| startTime | datetime? | Thời gian bắt đầu pha |
| endTime | datetime? | Thời gian kết thúc pha |
| ppTron | decimal | Hạt PP trơn (PP) |
| ppHdNhot | decimal | Hạt HD nhớt (PP) |
| ppLdpe | decimal | Hạt LDPE (PP) |
| ppDc | decimal | Hạt DC (PP) |
| ppAdditive | decimal | Phụ gia (PP) |
| ppColor | decimal | Hạt màu (PP) |
| ppOther | decimal | Hạt khác (PP) |
| ppRit | decimal | Hạt Rit (PP) |
| hdLldpe2320 | decimal | Hạt LLDPE 2320 (HD) |
| hdRecycled | decimal | Hạt tái chế (HD) |
| hdTalcol | decimal | Hạt Talcol (HD) |
| hdDc | decimal | Hạt DC (HD) |
| hdColor | decimal | Hạt màu (HD) |
| hdOther | decimal | Hạt khác (HD) |
| hdHd | decimal | Hạt HD (HD) |
| peAdditive | decimal | Phụ gia (PE) |
| peTalcol | decimal | Talcol (PE) |
| peColor | decimal | Hạt màu (PE) |
| peOther | decimal | Hạt khác (PE) |
| peLdpe | decimal | Hạt LDPE (PE) |
| peLldpe | decimal | Hạt LLDPE (PE) |
| peRecycled | decimal | Hạt tái chế (PE) |
| peDc | decimal | Hạt DC (PE) |
| shrinkRe707 | decimal | Tăng R8707 (Màng co) |
| shrinkSlip | decimal | Tăng Slip (Màng co) |
| shrinkStatic | decimal | Tăng tĩnh điện (Màng co) |
| shrinkDc | decimal | Hạt DC (Màng co) |
| shrinkTalcol | decimal | Talcol (Màng co) |
| shrinkOther | decimal | Hạt khác (Màng co) |
| shrinkLldpe | decimal | Hạt LLDPE (Màng co) |
| shrinkRecycled | decimal | Hạt tái chế (Màng co) |
| shrinkTangDai | decimal | Tăng dài (Màng co) |
| wrapRecycledCa | decimal | Hạt tái chế Ca (Màng chít) |
| wrapRecycledCb | decimal | Hạt tái chế Cb (Màng chít) |
| wrapGlue | decimal | Keo (Màng chít) |
| wrapColor | decimal | Hạt màu (Màng chít) |
| wrapDc | decimal | Hạt DC (Màng chít) |
| wrapLdpe | decimal | Hạt LDPE (Màng chít) |
| wrapLldpe | decimal | Hạt LLDPE (Màng chít) |
| wrapSlip | decimal | Hạt Slip (Màng chít) |
| wrapAdditive | decimal | Phụ gia (Màng chít) |
| wrapOther | decimal | Hạt khác (Màng chít) |
| wrapTangDaiC6 | decimal | Tăng dài C6 (Màng chít) |
| wrapTangDaiC8 | decimal | Tăng dài C8 (Màng chít) |
| evaPop3070 | decimal | POP 3070 (EVA) |
| evaLdpe | decimal | Hạt LDPE (EVA) |
| evaDc | decimal | Hạt DC (EVA) |
| evaTalcol | decimal | Hạt Talcol (EVA) |
| evaSlip | decimal | Slip (EVA) |
| evaStaticAdditive | decimal | Trợ tĩnh chống dính (EVA) |
| evaOther | decimal | Hạt khác (EVA) |
| evaTgc | decimal | TGC (EVA) |
| quantityKg | decimal | Sản lượng pha (Kg) |
| requiredDate | datetime? | Ngày cần hàng |
| isCompleted | bool | Xác nhận hoàn thành |
| status | int | Trạng thái |
| actualCompletionDate | datetime? | Ngày hoàn thành thực tế |
| delayReason | string? | Nguyên nhân chậm tiến độ |

---

## Business Logic

### Quản lý Lines
- Khi cập nhật, các line có `id` sẽ được cập nhật
- Các line không có `id` sẽ được tạo mới
- Các line không xuất hiện trong request body sẽ bị xóa

### Validation
- `productionDate` là bắt buộc
- `quantityKg` phải >= 0
- `workerId` phải tồn tại trong bảng Employees (nếu được cung cấp)
- `cardCode` phải tồn tại trong bảng BusinessPartners (nếu được cung cấp)

---

## Differences from GrainMixingProcess

Công đoạn pha hạt (Thổi) - **GrainMixingBlowingProcess** khác với công đoạn pha hạt thông thường **GrainMixingProcess** ở các điểm sau:

1. **Không có tính toán năng suất lao động**: GrainMixingBlowingProcess không có các trường `workerCount`, `totalHoursWorked` và `laborProductivity`
2. **Có thông tin máy thổi**: GrainMixingBlowingProcess có trường `blowingMachine` để lưu thông tin máy thổi được sử dụng
3. **Đơn giản hơn**: Tập trung vào việc theo dõi công đoạn pha hạt kết hợp với công đoạn thổi, không cần theo dõi chi tiết về nhân công và thời gian làm việc tổng thể
