# LouNexus Database Documentation

This document describes the database structure used by the LouNexus production tracking system.

The database is organized into multiple schemas that separate configuration data from operational production data and quality measurement data.

Schemas include:

- core
- prod
- quality
- admin

---

# Core Schema

The `core` schema contains configuration and structural data that defines the manufacturing environment.

These tables define factories, workstations, parts, and process requirements.

---

## factory

Represents a manufacturing location.

| Column | Type | Description |
|------|------|-------------|
| factory_id | integer | Primary key |
| name | text | Factory name |
| is_active | boolean | Indicates if factory is active |
| created_utc | timestamptz | Record creation timestamp |

---

## workstation_type

Defines logical workstation process types.

Examples:
- Diameter Grind
- Height Grind
- Final Inspection

| Column | Type | Description |
|------|------|-------------|
| workstation_type_id | integer | Primary key |
| name | text | Name of workstation type |
| description | text | Optional description |
| is_active | boolean | Indicates if workstation type is active |
| created_utc | timestamptz | Record creation timestamp |

---

## workstation

Represents a physical workstation within a factory.

| Column | Type | Description |
|------|------|-------------|
| workstation_id | integer | Primary key |
| factory_id | integer | FK → core.factory |
| workstation_type_id | integer | FK → core.workstation_type |
| name | text | Workstation name |
| is_active | boolean | Indicates if workstation is active |
| created_utc | timestamptz | Record creation timestamp |

---

## part

Represents a product part number.

| Column | Type | Description |
|------|------|-------------|
| part_id | integer | Primary key |
| part_number | text | Part number identifier |
| part_name | text | Human readable part name |
| part_description | text | Optional description |
| print_url | text | Link to engineering print |
| is_active | boolean | Indicates if part is active |
| created_utc | timestamptz | Record creation timestamp |

---

## part_workstation_requirement

Defines which workstation types are required for a part.

| Column | Type | Description |
|------|------|-------------|
| part_workstation_requirement_id | integer | Primary key |
| part_id | integer | FK → core.part |
| workstation_type_id | integer | FK → core.workstation_type |
| sequence_order | integer | Process order |
| is_required | boolean | Indicates if process is mandatory |
| created_utc | timestamptz | Record creation timestamp |

---

## part_measurement_spec

Defines measurement requirements for parts.

| Column | Type | Description |
|------|------|-------------|
| part_measurement_id | integer | Primary key |
| part_id | integer | FK → core.part |
| workstation_type_id | integer | FK → core.workstation_type |
| measurement_name | text | Name of measurement |
| target_value | numeric | Target measurement value |
| upper_limit | numeric | Upper tolerance limit |
| lower_limit | numeric | Lower tolerance limit |
| created_utc | timestamptz | Record creation timestamp |

---

## part_tracking_attribute

Defines tracking attributes required during production.

Examples:
- Material Lot
- Heat Number
- Supplier Lot

| Column | Type | Description |
|------|------|-------------|
| part_tracking_attribute_id | integer | Primary key |
| part_id | integer | FK → core.part |
| workstation_type_id | integer | FK → core.workstation_type |
| attribute_name | text | Name of attribute |
| is_required | boolean | Indicates if attribute is required |
| sort_order | integer | Display order |
| is_active | boolean | Indicates if attribute is active |
| created_utc | timestamptz | Record creation timestamp |

---

## reject_code

Defines reject categories.

Examples:
- CRK (Crack)
- BUR (Burr)

| Column | Type | Description |
|------|------|-------------|
| reject_code_id | integer | Primary key |
| code | text | Reject code |
| description | text | Description of defect |
| is_active | boolean | Indicates if code is active |
| created_utc | timestamptz | Record creation timestamp |

---

# Production Schema (prod)

Stores operational production data.

---

## inspection

Represents a production batch.

| Column | Type | Description |
|------|------|-------------|
| inspection_id | integer | Primary key |
| inspection_number | text | Unique inspection identifier |
| factory_id | integer | FK → core.factory |
| part_id | integer | FK → core.part |
| initial_quantity | integer | Initial batch quantity |
| status | text | Inspection status |
| notes | text | Optional notes |
| created_utc | timestamptz | Creation timestamp |
| closed_utc | timestamptz | Completion timestamp |

---

## station_event

Represents an action taken at a workstation.

| Column | Type | Description |
|------|------|-------------|
| station_event_id | integer | Primary key |
| inspection_id | integer | FK → prod.inspection |
| workstation_id | integer | FK → core.workstation |
| event_type | text | Type of event |
| good_quantity | integer | Quantity of good parts |
| notes | text | Optional notes |
| event_time_utc | timestamptz | Event timestamp |
| created_utc | timestamptz | Record creation timestamp |

---

## station_event_reject

Records reject quantities for a station event.

| Column | Type | Description |
|------|------|-------------|
| station_event_reject_id | integer | Primary key |
| station_event_id | integer | FK → prod.station_event |
| reject_code_id | integer | FK → core.reject_code |
| quantity | integer | Reject quantity |
| notes | text | Optional notes |
| created_utc | timestamptz | Record creation timestamp |

---

## station_event_attribute

Stores tracking attribute values entered at a station event.

| Column | Type | Description |
|------|------|-------------|
| station_event_attribute_id | integer | Primary key |
| station_event_id | integer | FK → prod.station_event |
| part_tracking_attribute_id | integer | FK → core.part_tracking_attribute |
| attribute_value | text | Entered value |
| notes | text | Optional notes |
| created_utc | timestamptz | Record creation timestamp |

---

# Quality Schema (quality)

Stores measurement data used for quality analysis.

---

## measurement_set

Represents a measurement session performed at a station event.

| Column | Type | Description |
|------|------|-------------|
| measurement_set_id | integer | Primary key |
| station_event_id | integer | FK → prod.station_event |
| part_measurement_id | integer | FK → core.part_measurement_spec |
| notes | text | Optional notes |
| created_utc | timestamptz | Record creation timestamp |

---

## measurement_value

Stores individual measurement readings.

| Column | Type | Description |
|------|------|-------------|
| measurement_value_id | integer | Primary key |
| measurement_set_id | integer | FK → quality.measurement_set |
| sample_index | integer | Measurement order |
| measured_value | numeric | Measured value |
| created_utc | timestamptz | Record creation timestamp |

---

# Admin Schema

Reserved for future administrative tables and system configuration.
