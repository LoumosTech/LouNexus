# LouNexus

LouNexus is a production tracking and measurement system designed to monitor manufacturing processes across multiple factories and workstations.

It allows operators to record production events, quality measurements, rejects, and tracking data for each inspection number while enforcing part-specific process requirements and measurement specifications.

LouNexus is designed to scale across multiple factories and hundreds of workstations while maintaining full traceability of production data.

---

# Overview

LouNexus tracks production through **inspection numbers**, which represent batches moving through various manufacturing processes.

Each inspection number is associated with a specific **part number**, and that part determines:

- Required workstation processes
- Measurement specifications (targets and limits)
- Required tracking attributes
- Reject tracking

Workstations record events such as:

- Measurement (CPK) entries
- Clockout / process completion
- Tracking data (lot numbers, etc.)
- Reject entries

All activity is stored as **station events**, allowing the system to maintain a complete historical record of production.

---

# Architecture

LouNexus uses a PostgreSQL database organized into several schemas.

[Docs here](./Docs/DATABASE.md)

## Core Schema

Contains configuration and structural data.

Tables include:

- `factory`
- `workstation_type`
- `workstation`
- `part`
- `part_workstation_requirement`
- `part_measurement_spec`
- `part_tracking_attribute`
- `reject_code`

These tables define the manufacturing structure and process requirements.

---

## Production Schema (`prod`)

Stores operational production data.

Tables include:

- `inspection`
- `station_event`
- `station_event_reject`
- `station_event_attribute`

This schema records all production activity tied to inspection numbers.

---

## Quality Schema (`quality`)

Stores measurement data used for quality control and CPK analysis.

Tables include:

- `measurement_set`
- `measurement_value`

These tables allow variable numbers of measurements per process and support statistical analysis of production quality.

---

## Inventory Schema (`inventory`)

The `inventory` schema tracks raw material inventory used in production.

Raw materials are stored as inventory lots that reference a part number and may optionally be assigned to a factory. If a material lot has not yet been sent to the production floor, the `factory_id` will be `NULL`.

This schema allows LouNexus to track available raw materials before they are consumed by production processes.

Tables include:

- `raw_material`

### Raw Material Inventory

The `raw_material` table represents individual raw material inventory lots.

Each row represents a quantity of raw material associated with a part number and lot number. Materials may optionally be assigned to a factory when they are distributed to the production floor.

Key characteristics:

- Each record represents a **material lot**
- Raw materials may exist in **stores (no factory assigned)**
- Materials can be **assigned to a factory for production use**
- Quantities are tracked per lot

Typical fields include:

- `raw_material_id`
- `part_id`
- `quantity`
- `lot_number`
- `material_description`
- `factory_id`
- `is_active`
- `created_utc`

---

## Admin Schema (`admin`)

Reserved for system-level configuration and administrative functionality such as:

- Application configuration
- System logs
- Future administrative tools

This schema currently contains no tables but exists for future expansion.

---

# Key Concepts

## Inspection Numbers

Inspection numbers represent production batches and are the central tracking unit of the system.

Each inspection number is associated with:

- A factory
- A part number
- An initial quantity
- A history of workstation events

---

## Workstation Types

Workstation types represent logical manufacturing processes such as:

- Diameter Grind
- Height Grind
- Final Inspection

Multiple factories may have workstations that implement the same workstation type.

This allows production to shift between factories without breaking process requirements.

---

## Station Events

Station events represent actions taken on an inspection number at a workstation.

Examples include:

- Recording measurement data
- Completing a process (clockout)
- Recording rejects
- Entering tracking attributes

Each station event becomes part of the permanent production history.

---

## Measurement System

Measurements are stored using a flexible structure that supports:

- Variable sample counts
- Multiple measurement types
- Part-specific measurement targets and limits

This allows LouNexus to support statistical process control and CPK analysis.

---

# Technology Stack

- PostgreSQL
- C#
- WinForms (planned client application)

---

# Future Development

Planned areas of development include:

- Workstation client application
- Production control tools
- CPK analysis and reporting
- Process dashboards
- Configuration management tools

---

# Project Status

Early development.

Database schema design is currently complete and application development will follow.
