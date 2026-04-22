# PeopleTray - Comprehensive Features Documentation
## Business Analyst Level Analysis - Production & TeamTray Modules

**Website:** https://staging.peopletray.com  
**Last Updated:** April 16, 2026  
**Account:** sa@miningpatch.com  
**Documentation Level:** Business Analyst (BA)

---

## Table of Contents
1. [Executive Summary](#executive-summary)
2. [Production Module - Detailed Analysis](#production-module---detailed-analysis)
3. [TeamTray Module - Detailed Analysis](#teamtray-module---detailed-analysis)
4. [Integration & Data Flow](#integration--data-flow)
5. [Business Processes](#business-processes)
6. [Key Reports & Dashboards](#key-reports--dashboards)
7. [Configuration & Administration](#configuration--administration)

---

## Executive Summary

PeopleTray is an enterprise-grade integrated management platform designed for complex operations in resource-intensive industries (mining, drilling, etc.). The system provides end-to-end management of operational activities and human resources through two primary modules:

### Business Value Proposition:
- **Production Module:** Enables complete operational control of mining/production activities from planning through execution and compliance
- **TeamTray Module:** Provides integrated human resource management tied directly to operational requirements
- **Integrated Platform:** Eliminates data silos between operations and personnel management, enabling real-time visibility across the organization

### Strategic Benefits:
1. **Operationally Driven:** Resources and activities are directly connected to production needs
2. **Compliance Ready:** Built-in documentation, audit trails, and approval workflows meet regulatory requirements
3. **Real-time Visibility:** Dashboard widgets provide instant status of critical operations and personnel matters
4. **Scalable Configuration:** Extensive settings allow customization to organizational structure and processes

---

## Production Module - Detailed Analysis

The Production Module serves as the operational command center for all mining and production activities. It encompasses the complete lifecycle of production operations from planning through execution, documentation, and compliance.

### Module Purpose & Business Objectives:
- **Operational Planning:** Define production schedules, resource allocation, and equipment deployment
- **Activity Tracking:** Record all production activities with detailed documentation
- **Cost Management:** Track costs through invoices and contract management
- **Safety & Compliance:** Maintain comprehensive documentation for regulatory compliance
- **Performance Monitoring:** Generate summaries and reports for operational insights
- **Asset Management:** Manage equipment, materials, and drill bits throughout their lifecycle

---

### 1. MATERIAL MOVEMENTS MANAGEMENT

#### Business Context:
Material movements represent the core operational activity in mining operations. This includes the movement of extracted materials, supplies, equipment, and resources between locations.

**Key Features:**
- **Material Movements (List View):** Complete inventory of all material transfers
  - Date and time of movement
  - Source location (where material originates)
  - Destination location (where material goes)
  - Material type and quantity
  - Personnel responsible
  - Status tracking

- **Movements Summary:** Aggregated view of movement activities
  - Summarizes movements by location, time period, material type
  - Provides trend analysis
  - Identifies bottlenecks and inefficiencies
  - Supports capacity planning

**Business Value:**
- **Operational Efficiency:** Identify movement patterns and optimize logistics
- **Cost Control:** Track material handling costs
- **Inventory Accuracy:** Maintain real-time material location data
- **Accountability:** Record who moved what, when, and where
- **Compliance:** Audit trail for regulatory requirements

**User Workflows:**
1. Operator records material movement at the point of operation
2. System tracks movement in real-time
3. Supervisor can view movements summary for shift analysis
4. Management uses movement data to optimize operations
5. Finance uses movement records for invoicing and cost allocation

#### Configuration (Movements Settings):
- Define movement types (e.g., Extraction, Transport, Processing)
- Set movement policies and approved routes
- Configure movement workflows and approvals

---

### 2. SHIFTS MANAGEMENT

#### Business Context:
Production operates on defined shifts. Shift management controls when production occurs, who participates, and what activities take place.

**Key Features:**
- **Add Shift:** Create new shift records including:
  - Shift date and time (start/end)
  - Shift type (day, night, extended, etc.)
  - Location/production area
  - Assigned personnel
  - Planned activities
  - Supervisor/foreman assignment

- **Shift Tracking:** Monitor shift progress
  - Real-time activity recording
  - Personnel check-in/out
  - Activity completion status
  - Issues and delays logging

**Business Value:**
- **Production Scheduling:** Plan and track production capacity
- **Resource Allocation:** Ensure adequate staffing for each shift
- **Safety Oversight:** Supervisor presence and activity monitoring
- **Cost Analysis:** Calculate shift-based costs
- **Performance Tracking:** Measure shift productivity

**Key Business Metrics Enabled:**
- Shift utilization rate
- Personnel availability per shift
- Activity completion percentage per shift
- Shift downtime analysis

---

### 3. DRILL BITS MANAGEMENT

#### Business Context:
Drill bits are critical consumable assets in drilling operations. Proper management ensures operational continuity and cost optimization.

**Key Features:**
- **Inventory Tracking:** Maintain drill bit inventory
  - Type and size specifications
  - Current location
  - Status (new, in-use, worn, retired)
  - Serial number tracking

- **Usage Recording:** Log drill bit deployment
  - Which hole drilled
  - Depth drilled
  - Time in operation
  - Performance observations

- **Lifecycle Management:**
  - New bit procurement
  - Deployment to active drilling
  - Monitoring of wear and performance
  - Replacement scheduling
  - Retirement and disposal

- **Cost Tracking:**
  - Purchase cost per bit
  - Usage cost per meter/hour
  - Actual vs. expected lifespan
  - Failure analysis

**Business Value:**
- **Asset Optimization:** Maximize drill bit lifespan
- **Operational Planning:** Know bit availability and location
- **Cost Control:** Track consumable spending
- **Quality Control:** Monitor performance trends
- **Preventive Maintenance:** Schedule replacements before failure

**Critical KPIs:**
- Average drill bit lifespan (meters/hours)
- Cost per meter drilled
- Unplanned replacement rate
- Inventory turnover

---

### 4. HOLE SURVEYS

#### Business Context:
Hole surveys document geological and drilling data for each hole drilled. Critical for mining planning, safety, and resource estimation.

**Key Features:**
- **Survey Data Recording:**
  - Hole location (GPS coordinates or grid reference)
  - Hole depth
  - Angle/inclination
  - Geological observations
  - Sample data (if collected)
  - Assay results (metal content)

- **Survey Documentation:**
  - Depth intervals and measurements
  - Rock type description
  - Structural features observed
  - Water table location
  - Quality assessment

**Business Value:**
- **Geological Intelligence:** Understand mineral distribution
- **Mine Planning:** Optimize extraction strategy
- **Reserve Estimation:** Calculate ore reserves
- **Safety Compliance:** Document hole conditions
- **Quality Assurance:** Maintain survey standards

**Integration Points:**
- Links to Drill Bits used in that hole
- Connects to Production Planning
- Feeds into Reserve Estimation systems
- Supports Safety & Environmental compliance

---

### 5. CHARGE SHEETS

#### Business Context:
Charge sheets document explosive charges used in blasting operations. Essential for safety, compliance, and cost tracking.

**Key Features:**
- **Blast Planning:**
  - Blast location and date/time
  - Holes to be blasted (reference to Hole Surveys)
  - Explosive type and quantity
  - Detonation sequence
  - Blast parameters

- **Safety Documentation:**
  - Personnel authorizations
  - Safety clearances
  - Warning and isolation procedures
  - Environmental considerations

- **Post-Blast Recording:**
  - Actual explosives used
  - Detonation timing
  - Results and fragmentation
  - Any incidents or observations

- **Cost Allocation:**
  - Explosive costs per blast
  - Material moved per blast
  - Efficiency metrics

**Business Value:**
- **Safety Compliance:** Meets mining safety regulations
- **Cost Management:** Track explosive consumption
- **Blast Optimization:** Improve fragmentation and productivity
- **Audit Trail:** Complete documentation for regulators
- **Performance Analysis:** Optimize blasting patterns

**Critical Compliance Requirements:**
- Licensed blaster authorization
- Environmental impact assessment
- Incident documentation
- Historical records for audits

---

### 6. PLODs (Production Line Operations Documents)

#### Business Context:
PLODs provide comprehensive documentation of production line operations, essential for process compliance and continuous improvement.

**Key Features:**
- **Operational Documentation:**
  - Process steps performed
  - Parameters and settings used
  - Output produced
  - Issues encountered
  - Time and duration
  - Personnel responsible

- **Compliance Recording:**
  - Regulatory requirements met
  - Safety protocols followed
  - Quality checks performed
  - Environmental controls applied

- **Performance Tracking:**
  - Actual vs. planned operation
  - Downtime recording
  - Root cause analysis
  - Corrective actions taken

**Business Value:**
- **Process Control:** Ensure consistent operations
- **Quality Assurance:** Maintain output standards
- **Compliance Proof:** Demonstrate regulatory adherence
- **Continuous Improvement:** Identify optimization opportunities
- **Historical Record:** Track operational decisions

---

### 7. INVOICES MANAGEMENT

#### Business Context:
Invoices track financial transactions related to production activities, including contractor charges, material purchases, and service costs.

**Key Features:**
- **Invoice Creation:**
  - Vendor/contractor information
  - Services or materials provided
  - Quantity and unit costs
  - Date of service/delivery
  - Invoice reference numbers

- **Cost Allocation:**
  - Assign costs to specific projects
  - Link to material movements
  - Associate with shifts or activities
  - Production site/location tracking

- **Invoice Tracking:**
  - Payment status
  - Approval workflow
  - Due dates
  - Payment history

- **Financial Reporting:**
  - Cost summaries by vendor
  - Cost trends over time
  - Budget variance analysis
  - Cost per unit of production

**Business Value:**
- **Cost Control:** Monitor and manage expenses
- **Budget Accuracy:** Track actual vs. budgeted costs
- **Vendor Management:** Monitor contractor performance and pricing
- **Financial Reporting:** Support accounting and audit functions
- **Profitability Analysis:** Calculate true production costs

**Integration Points:**
- Links to Material Movements (for associated costs)
- Connected to Contracts for pricing verification
- Feeds into Equipment Plan for asset costs
- Supports cost allocation to KPIs

---

### 8. CONTRACTS & CONTRACT PLAN

#### Business Context:
Contracts define agreements with contractors and vendors. Contract Planning allocates equipment and resources according to contract terms.

**Features:**

**Contracts Management:**
- Contract details (vendor, dates, terms)
- Scope of work
- Pricing terms and rates
- Performance requirements
- Payment milestones
- Compliance requirements

**Contract Equipment Plan:**
- Equipment allocation by contract
- Resource scheduling aligned to contract
- Performance tracking vs. contract terms
- Cost tracking against contract rates
- Compliance verification

**Business Value:**
- **Vendor Management:** Track all contractual relationships
- **Cost Management:** Ensure billing matches contracted rates
- **Performance Monitoring:** Verify contractor delivers per agreement
- **Risk Management:** Monitor contract compliance
- **Planning:** Allocate resources based on contract commitments

**Contract Lifecycle:**
1. Contract creation with terms
2. Equipment plan developed for contract
3. Activities performed under contract
4. Invoices generated per contract terms
5. Contract completion and closure

---

### 9. EQUIPMENT PLAN

#### Business Context:
Equipment planning manages the overall allocation, deployment, and utilization of equipment across the operation.

**Key Features:**
- **Equipment Inventory:**
  - Equipment type and specifications
  - Current location and status
  - Maintenance schedule
  - Operational capacity
  - Depreciation/valuation

- **Deployment Planning:**
  - Assign equipment to shifts/locations
  - Schedule equipment moves
  - Plan maintenance windows
  - Monitor utilization rates

- **Performance Tracking:**
  - Hours/meters of operation
  - Actual vs. expected productivity
  - Maintenance history
  - Downtime analysis

- **Cost Management:**
  - Ownership costs
  - Operating costs per unit
  - Maintenance expenses
  - Depreciation tracking

**Business Value:**
- **Operational Efficiency:** Maximize equipment utilization
- **Maintenance Planning:** Prevent unexpected downtime
- **Cost Optimization:** Track true equipment costs
- **Capacity Planning:** Know available equipment
- **Asset Management:** Proper tracking and control

**Key Metrics:**
- Equipment utilization rate
- Mean time between failures (MTBF)
- Cost per operating hour
- Availability percentage

---

### 10. BUSINESS PLAN

#### Business Context:
Strategic planning tool for overall business objectives, initiatives, and resource allocation.

**Features:**
- Strategic objectives definition
- Initiative planning
- Resource allocation
- Timeline and milestones
- Success metrics
- Budget allocation

**Integration with Operations:**
- Links operational performance to business objectives
- Ensures activities align with strategy
- Supports resource prioritization

---

### 11. INCOMING REPORTS

#### Business Context:
Reports submitted by field personnel or contractors awaiting review and approval.

**Features:**
- Report type (activity, incident, progress, etc.)
- Submission timestamp and submitter
- Report content and attachments
- Status tracking
- Approval workflow
- Comments and feedback

**Business Value:**
- **Real-time Information:** Get visibility of field activities
- **Approval Control:** Manage report quality and consistency
- **Compliance:** Ensure all required reports are submitted
- **Communication:** Feedback channel to field teams

---

### Production Module Settings & Configuration

#### Stock Catalogue Configuration:
Defines all materials, consumables, and products managed in the system.
- Material categories
- Units of measure
- Pricing
- Safety data
- Reorder points

#### Production Types:
Classification of different production activities or areas.
- Mine type (underground, open pit, etc.)
- Production line type
- Operational characteristics
- Safety requirements

#### Activities:
Defines all possible activities that can occur in production.
- Drilling, blasting, hauling, processing
- Maintenance activities
- Support activities
- Safety activities

#### Equipment Types:
Classification of all equipment used.
- Heavy equipment (loaders, trucks, etc.)
- Drilling equipment
- Processing equipment
- Support equipment

#### Labour Types:
Classification of personnel by role and skills.
- Operators, supervisors, specialists
- Licensed/certified positions
- Support roles

#### Units of Measure:
Define measurement standards.
- Metric vs. imperial
- Weight, volume, distance
- Time measurements

#### KPIs (Key Performance Indicators):
Configure metrics tracked for operational excellence.
- Production volume per shift
- Safety incidents
- Equipment utilization
- Cost per unit
- Drill rate
- Blast efficiency

---

## TeamTray Module - Detailed Analysis

The TeamTray Module provides comprehensive human resource and team management, tightly integrated with operational requirements. Unlike traditional HR systems, TeamTray is production-centric, linking personnel directly to operational activities and requirements.

### Module Purpose & Business Objectives:
- **Workforce Planning:** Align personnel with operational needs
- **Compliance Management:** Ensure proper certifications and training
- **Resource Allocation:** Assign qualified personnel to activities
- **Performance Tracking:** Monitor individual and team productivity
- **Development:** Support career progression and skills development
- **Safety & Health:** Manage safety protocols and health compliance

---

### 1. PEOPLE MANAGEMENT

#### Business Context:
Central personnel database and management system, connected to operational roles and responsibilities.

**Key Features:**

**Employee Directory:**
- Personal information (name, contact, ID)
- Role and position
- Department/reporting structure
- Employment status (active, leave, terminated)
- Start date and tenure

**Qualifications & Certifications:**
- Current certifications (blasting, equipment operation, etc.)
- Expiration dates and renewal schedules
- Training history
- Competency assessments
- License tracking (driver's license, heavy equipment, etc.)

**Organizational Structure:**
- Reporting lines and hierarchy
- Department assignments
- Team membership
- Supervisory relationships

**Personnel Grouping:**
- Create groups by role (operators, supervisors, etc.)
- Location-based groups
- Skill-based groups
- Functional groups

**Business Value:**
- **Operational Readiness:** Know who has required skills/certifications
- **Compliance:** Track certification validity
- **Planning:** Understand available workforce
- **Safety:** Ensure only certified personnel perform activities
- **Development:** Identify training needs

**Critical Integration:**
- Links to Crew Plan (crew assignments)
- Connects to Rosters (availability tracking)
- Feeds into Leave/Change Requests
- Supports shift assignments
- Required for safety compliance

---

### 2. CREW PLANNING

#### Business Context:
Assigns qualified personnel to specific production locations, equipment, or assets for defined periods.

**Key Features:**

**Crew Composition:**
- Define crew for specific asset (drill rig, loader, processing facility, etc.)
- Specify crew size and roles
- Skill requirements per position
- Experience requirements

**Crew Assignment:**
- Assign specific personnel to crew roles
- Define assignment duration
- Document certifications are current
- Track role responsibilities

**Crew Rotation:**
- Plan crew changes and rotations
- Manage relief and replacement
- Track crew continuity
- Manage fatigue and safety

**Performance Tracking:**
- Measure crew output
- Track incidents per crew
- Monitor efficiency metrics
- Identify training needs

**Business Value:**
- **Operational Continuity:** Ensure required skills available
- **Safety Assurance:** Qualified personnel on equipment
- **Productivity:** Right skills for the job
- **Risk Management:** Track crew stability
- **Planning:** Know personnel location and assignments

**Integration Points:**
- Linked to roster data (availability)
- Connected to equipment tracking
- Feeds into shift planning
- Supports safety compliance

**Critical Business Process:**
1. Define crew requirements for asset
2. Identify available personnel with skills
3. Assign personnel to crew positions
4. Monitor crew performance
5. Plan rotations based on roster/leave
6. Update crew when personnel change

---

### 3. ROSTERING & SCHEDULING

#### Business Context:
Work scheduling system managing shift assignments, availability, and duty hours.

**Key Features:**

**Roster Creation:**
- Define shift patterns (day, night, swing, etc.)
- Assign personnel to shifts
- Specify shift hours and location
- Note role/responsibility in shift

**Schedule Management:**
- View personnel availability
- Identify gaps in coverage
- Balance workload across team
- Plan for absences (leave, training)

**Duty Tracking:**
- Record actual attendance
- Track hours worked
- Monitor overtime
- Enforce fatigue management rules

**Leave Planning:**
- Plan for approved leave periods
- Identify replacement personnel
- Maintain continuity
- Balance team capacity

**Reporting:**
- Shift coverage reports
- Personnel utilization
- Overtime tracking
- Absence analysis

**Business Value:**
- **Compliance:** Adhere to labor regulations
- **Safety:** Manage fatigue levels (critical in mining)
- **Cost Control:** Optimize staffing costs
- **Operational Continuity:** Maintain adequate coverage
- **Planning:** Know personnel availability
- **Legal:** Document duty hours for labor law compliance

**Integration with Operations:**
- Rosters feed into Crew Plan
- Support Shift creation in Production
- Enable leave request processing
- Provide data for cost allocation
- Support Travel requests

---

### 4. LEAVE & CHANGE REQUESTS

#### Business Context:
Workflow system managing employee absences, employment changes, and exceptions to normal schedules.

**Key Features:**

**Leave Requests:**
- **Leave Types:** Annual leave, sick leave, unpaid leave, special leave, etc.
- **Request Process:**
  - Employee submits request with dates
  - System checks roster impact
  - Supervisor reviews for coverage
  - Manager approves/rejects
  - System updates roster
  
- **Approval Workflow:**
  - Multi-level approval if needed
  - Comments and justifications
  - Conflict detection (coverage gaps)
  - Audit trail of decisions

- **Leave Balances:**
  - Track available leave per employee
  - Monitor accrual
  - Carryover rules
  - Expiry notifications

**Change Requests:**
- **Employment Changes:**
  - Position changes
  - Department transfers
  - Promotion/demotion
  - Termination notice
  - Salary adjustments
  
- **Request Management:**
  - Supervisor recommendation
  - HR approval
  - Effective date planning
  - System updates upon approval

**Business Value:**
- **Scheduling Certainty:** Know approved absences in advance
- **Compliance:** Proper record of leave and changes
- **Planning:** Ensure coverage for approved leave
- **Fairness:** Consistent approval process
- **Risk Management:** Document employment changes
- **Legal:** Employment law compliance

**Integration with Operations:**
- Leave requests impact crew availability
- System adjusts roster upon approval
- Affects shift scheduling
- Impacts cost calculations
- Influences crew planning

**Workflow Steps:**
1. Employee submits leave/change request
2. System validates against leave balance and roster
3. Supervisor reviews for operational impact
4. Manager approves/rejects with comments
5. System updates roster and crew assignments
6. Notification to affected personnel
7. Audit record of decision

---

### 5. TRAVEL REQUEST MANAGEMENT

#### Business Context:
Manages employee travel arrangements for work-related trips, including flights, accommodations, and associated logistics.

**Key Features:**

**Travel Request Submission:**
- Destination and dates
- Purpose of travel
- Budget authorization
- Estimated costs
- Justification

**Approval Workflow:**
- Supervisor recommendation
- Manager approval
- Budget verification
- Safety/security approval if international

**Travel Arrangements:**
- Flight booking and tracking
- Accommodation assignment
- Ground transportation
- Travel allowance authorization
- Meal and incidental rates

**Rooms & Flights Management:**
- Preferred hotel selection
- Room allocation
- Flights selection and booking
- Alternative arrangements if needed

**Travel Documentation:**
- Passport/visa requirements
- Travel insurance
- Safety briefings
- Emergency contacts
- Location tracking

**Cost Management:**
- Budget tracking
- Expense limits by level
- Reimbursement processing
- Cost allocation to projects

**Business Value:**
- **Cost Control:** Budget management for travel
- **Compliance:** Approval for business travel
- **Safety:** Track employee location
- **Planning:** Coordinate work across locations
- **Efficiency:** Streamlined travel booking
- **Audit:** Full record for expense management

**Integration Points:**
- Linked to Crew Planning (crew relocations)
- Supports training off-site attendance
- Enables conference/trade show participation
- Documents contractor/visitor management

---

### 6. TRAINING & DEVELOPMENT

#### Business Context:
Manages employee training and capability development, with emphasis on operational certifications and safety training.

**Key Features:**

**Training Programs:**
- Training name and description
- Prerequisites and skill levels
- Duration and format (classroom, on-the-job, online)
- Certification upon completion
- Training provider/instructor
- Cost and budget allocation

**Enrollment Management:**
- Employee enrollment in courses
- Prerequisites verification
- Scheduling and attendance
- Mandatory vs. optional training
- Competency prerequisites

**Completion Tracking:**
- Completion status and dates
- Certifications issued
- Assessment results
- Competency validation

**Development Plans:**
- Individual development paths
- Career progression training
- Skill gap identification
- Learning objectives
- Timeline and milestones

**Certification Management:**
- Certification types (equipment operation, blasting, etc.)
- Validity periods
- Renewal requirements
- Expiry tracking and alerts
- Compliance verification

**Business Value:**
- **Skill Development:** Build organizational capability
- **Compliance:** Required training and certifications
- **Safety:** Ensure personnel competent in role
- **Career Growth:** Support employee development
- **Succession Planning:** Identify and develop future leaders
- **Risk Mitigation:** Proper qualifications documented

**Critical Integration:**
- Training completion required before crew assignment
- Certification expiry must be tracked for compliance
- Training history appears in personnel records
- Supports safety compliance audits
- Links to KPI performance tracking

**Operational Impact:**
- Cannot assign to equipment without certification
- Refresh training triggers notifications
- Performance tracking identifies development needs
- Career progression dependent on training

---

### 7. ACTIVITY REPORTS

#### Business Context:
Field personnel submit activity reports documenting work performed, challenges encountered, and observations.

**Key Features:**

**Report Submission:**
- Date and shift identification
- Activities performed (with reference to activities list)
- Hours spent per activity
- Material handled
- Equipment used
- Personnel involved
- Observations and comments

**Report Content:**
- Narrative description of work
- Issues and challenges
- Safety observations
- Quality notes
- Equipment performance observations
- Suggestions for improvement

**Approval Process:**
- Supervisor reviews for completeness
- Validates against recorded data
- Provides feedback or approvals
- Comments and corrections
- Sign-off and authorization

**Performance Tracking:**
- Links to KPIs
- Identifies non-standard activities
- Tracks efficiency
- Highlights issues for resolution

**Business Value:**
- **Accountability:** Record of activities performed
- **Visibility:** Real-time understanding of operations
- **Continuous Improvement:** Identify issues and improvements
- **Communication:** Link between field and management
- **Quality Assurance:** Document work standards
- **Compliance:** Audit trail of operations

**Integration with Production:**
- Supplements production data from monitoring systems
- Provides context to production records
- Feeds into performance analysis
- Supports safety incident investigation

---

### 8. JOURNEYS & CAREER MANAGEMENT

#### Business Context:
Tracks employee career progression, skill development, and career pathways within the organization.

**Key Features:**

**Career Pathways:**
- Define progression from entry to senior roles
- Identify skill and experience requirements
- Map training and development steps
- Timeline expectations
- Success criteria

**Individual Journeys:**
- Current position and level
- Identified career direction
- Skill development plan
- Training and experience milestones
- Goal setting and tracking
- Progress reviews

**Capability Tracking:**
- Current competencies
- Skill levels and proficiency
- Experience hours in roles
- Performance assessments
- Readiness for advancement

**Business Value:**
- **Retention:** Career growth opportunities attract and retain talent
- **Succession Planning:** Identify and develop future leaders
- **Capability Building:** Strategic skill development
- **Engagement:** Employees understand progression
- **Organizational Knowledge:** Track expertise and experience

---

### 9. ANNOUNCEMENTS

#### Business Context:
Organization-wide and team-specific communication tool for important updates and information.

**Features:**
- Create announcements by author
- Target audience (organization-wide or department)
- Publishing date and duration
- Message content
- Attachments
- Acknowledgment tracking

**Business Value:**
- **Communication:** Disseminate important information
- **Compliance:** Communicate policy changes
- **Engagement:** Keep team informed
- **Record:** Maintain communication history
- **Acknowledgment:** Verify receipt of critical information

---

### 10. CREW PLAN (Asset-Based Resource Management)

#### Business Context:
Links personnel to specific physical assets or locations for operational periods.

**Key Features:**
- Asset identification (equipment, location, facility)
- Crew configuration for that asset
- Personnel assignments with roles
- Assignment duration
- Performance tracking

**Business Value:**
- **Operational Management:** Know crew on each asset
- **Safety:** Accountability and supervision tracking
- **Performance:** Asset-based productivity
- **Planning:** Schedule maintenance around crewing

---

### 11. ROSTERS, FLIGHTS, ROOMS

#### Business Context:
Operational logistics for personnel scheduling, transportation, and accommodation.

**Rosters:** Shift scheduling (detailed above)

**Flights:** Management of employee air travel
- Preferred airlines and routes
- Travel arrangements
- Seat preferences
- Boarding pass management
- Travel documentation

**Rooms:** Accommodation management
- Room allocation
- Accommodation arrangements
- Meal services
- Check-in/check-out
- Facility management

---

### TeamTray Module Settings & Configuration

**People Management:**
- Position/role definitions
- Department structure
- Reporting relationships
- Access permissions
- Personnel categories

**Training:**
- Training program definitions
- Certification types and validity
- Mandatory training identification
- Competency requirements per role
- Training provider management

**Leave Policies:**
- Leave types and accrual
- Approval levels
- Carry-over rules
- Leave year definition
- Country-specific compliance

**Travel:**
- Approved airlines and hotels
- Budget limits by level
- Approval authority matrix
- Travel policy rules
- Cost allocation codes

---

## Integration & Data Flow

### Production-TeamTray Integration

The true power of PeopleTray lies in the tight integration between Production and TeamTray modules, creating a unified operational and personnel management system.

#### Key Integration Points:

**1. Personnel Assignment to Operations:**
- Personnel records → Crew Plan → Equipment/Asset assignment
- Active crew assignment enables shift assignment
- Only certified personnel can be assigned to specific equipment
- Crew availability feeds into production scheduling

**2. Shift Planning & Coverage:**
- Roster provides personnel availability
- Equipment Plan requires crew assignment
- Shift creation links to Crew Plan
- Leave requests affect shift coverage
- System prevents scheduling conflicts

**3. Material Movement & Personnel:**
- Each material movement can be assigned to a shift/crew
- Personnel responsible for movement recorded
- Activity reports support movement data
- Fuel/supply consumption tracked per crew

**4. Training & Equipment Certification:**
- Equipment operation requires specific training
- System blocks uncertified personnel assignment
- Training expiry triggers alerts
- Compliance verification automatic

**5. Cost Allocation:**
- Personnel costs allocated to shifts
- Shift costs allocated to equipment/contracts
- Equipment costs allocated to material movements
- Complete cost traceability

**6. Safety & Incident Tracking:**
- Safety incidents documented per crew
- Training linked to incident types
- Competency tracking informs incident analysis
- Safety metrics tied to personnel performance

### Data Flow Examples:

**Example 1: New Equipment Introduction**
1. New equipment added to Equipment Plan
2. Required training identified
3. Personnel enrolled in training program
4. Upon completion, personnel eligible for assignment
5. Crew plan updated with trained personnel
6. Shifts can now be assigned with that equipment
7. Activity reports track equipment performance

**Example 2: Leave and Coverage**
1. Personnel submit leave request
2. System identifies affected shifts
3. Supervisor reviews coverage impact
4. Leave approved with replacement crew identified
5. Roster updated automatically
6. Shift assignments adjusted
7. Crew plan reflects new personnel
8. Equipment remains covered

**Example 3: Compliance & Certification Renewal**
1. Training certification added to personnel record
2. Expiry date tracked (e.g., 12 months)
3. 30 days before expiry, alert triggered
4. Training refresher course scheduled
5. Completion updates personnel record
6. Crew assignment remains valid
7. Historical record maintained for audit

---

## Business Processes

### Process 1: Production Planning Cycle

**Objective:** Plan production activities for a defined period (week, month)

**Steps:**
1. Forecast production requirements (ore, material targets)
2. Identify equipment required and availability
3. Check crew availability via roster
4. Plan equipment allocation (Equipment Plan)
5. Schedule shifts based on requirements
6. Assign crews to equipment per schedule
7. Plan drill patterns and locations
8. Schedule contractor support (contracts)
9. Communicate plan via announcements
10. Monitor actual vs. planned via movements summary

**Responsibilities:**
- Production Manager: Overall planning
- Equipment Manager: Equipment availability
- HR Manager: Personnel availability
- Supervisors: Crew assignment details

**Key Decision Points:**
- Equipment sufficient for plan?
- Personnel availability meets requirements?
- Contractor capacity available?
- Cost within budget?

### Process 2: Shift Execution & Documentation

**Objective:** Execute planned shift and document activities

**Steps:**
1. Supervisor reviews assigned equipment and crew
2. Crew arrives and checks in
3. Morning safety meeting with crew
4. Shift activities performed (drilling, blasting, hauling, etc.)
5. Material movements recorded in real-time
6. Issues logged and escalated
7. Supervisor monitors progress
8. End-of-shift handover meeting
9. Activity report submitted by supervisor
10. System calculates shift metrics (volume, costs, productivity)

**Documentation Requirements:**
- Shift start/end times
- Personnel present
- Equipment used
- Material moved
- Issues encountered
- Safety observations
- Activity completion %

**Key Metrics Generated:**
- Material volume moved
- Equipment hours utilized
- Cost per unit produced
- Safety incident rate
- Productivity vs. plan

### Process 3: Drill Bit Management Lifecycle

**Objective:** Manage drill bit procurement, deployment, and replacement

**Steps:**

**Planning Phase:**
1. Forecast drill bit requirements (based on drilling plan)
2. Identify bit types and quantities needed
3. Verify stock availability
4. Plan procurement if shortage

**Deployment Phase:**
1. Bit selected from inventory
2. Assigned to specific drill site/equipment
3. Installation recorded with date/time
4. Hole surveys link to bit used
5. Performance monitoring during use

**Monitoring Phase:**
1. Operator observes bit performance
2. Drill rate tracking
3. Bit wear assessment
4. Issues/problems documented
5. Replacement decision made

**Replacement Phase:**
1. Worn bit removed and returned to inventory
2. New bit installed
3. Old bit evaluated (repair vs. scrap)
4. Cost recorded
5. Performance analysis

**Retirement Phase:**
1. End-of-life bits removed from inventory
2. Disposal/recycling recorded
3. Cost written off
4. Historical data archived

**Key Measurable Outcomes:**
- Metres drilled per bit
- Hours per bit
- Cost per metre drilled
- Bit life vs. forecast
- Downtime due to bit failure

### Process 4: Training & Compliance

**Objective:** Ensure personnel have required competencies and certifications

**Steps:**

**Identification Phase:**
1. Competency framework defined by role
2. Assessment identifies skill gaps
3. Training needs identified
4. Training program selected (internal/external)

**Enrollment Phase:**
1. Personnel enrolled in program
2. Prerequisites verified
3. Schedule coordinated (roster consideration)
4. Travel arranged if needed

**Execution Phase:**
1. Training delivery (classroom, online, on-job)
2. Attendance tracked
3. Assessments completed
4. Performance evaluated

**Certification Phase:**
1. Competency verified
2. Certification issued
3. Personnel record updated
4. Expiry date recorded
5. Automated renewal reminder scheduled

**Compliance Verification:**
1. Crew assignment verified for certifications
2. Expiry date checked (pre-assignment)
3. Compliance report generated
4. Gap identification and escalation

**Outcomes:**
- Personnel competency levels mapped
- Certification compliance verified
- Training cost tracked
- Skill development documented
- Succession pipeline identified

### Process 5: Leave & Relief Planning

**Objective:** Manage personnel absences while maintaining operational continuity

**Steps:**

**Request Phase:**
1. Personnel submit leave request
2. System shows leave balance
3. Reason and dates specified
4. Constraints identified (crew critically short, etc.)

**Review Phase:**
1. Supervisor reviews for operational impact
2. Leave balance verified
3. Coverage feasibility assessed
4. Alternative dates suggested if needed

**Approval Phase:**
1. Manager approves/rejects
2. Comments provided
3. Decision communicated
4. System updates leave balance

**Planning Phase:**
1. Relief personnel identified
2. Crew adjustments made
3. Shift assignments updated
4. Roster communicated
5. Training arranged if needed for relief

**Handover Phase:**
1. Knowledge transfer occurs
2. Substitute checks crew equipment
3. Outgoing personnel documents status
4. Equipment condition verified

**Return Phase:**
1. Personnel returns from leave
2. Orientation briefing provided
3. System reactivates original assignments
4. Performance metrics reviewed

**Outcomes:**
- Zero unplanned downtime due to absence
- Adequate coverage maintained
- Proper handover documented
- Personnel engagement improved
- Compliance with labor regulations

### Process 6: Invoice & Cost Management

**Objective:** Track costs and manage financial transactions

**Steps:**

**Cost Incurrence:**
1. Activity performed (material movement, shift, etc.)
2. Contractor provides service
3. Material purchased
4. Equipment used

**Invoice Receipt:**
1. Vendor submits invoice
2. Details captured (date, amount, service)
3. Receipt verified against PO
4. Cost allocation recorded

**Approval:**
1. Cost center manager reviews
2. Activity verification (service delivered?)
3. Rate verification (matches agreement?)
4. Approval for payment

**Recording:**
1. System records invoice
2. Allocates cost to project/shift/asset
3. Updates financial records
4. Affects KPI calculations

**Analysis:**
1. Cost vs. budget analysis
2. Variance investigation
3. Trend analysis
4. Optimization opportunities identified

**Outcomes:**
- Accurate cost tracking by activity
- Vendor performance monitoring
- Budget variance management
- Profitability analysis
- Cost control insights

### Process 7: Safety & Compliance Reporting

**Objective:** Maintain safety standards and regulatory compliance

**Steps:**

**Incident Recording:**
1. Incident occurs/is observed
2. Description and context recorded
3. Personnel involved documented
4. Severity assessed
5. Immediate action taken

**Investigation:**
1. Incident reviewed by supervisor
2. Root cause analysis performed
3. Contributing factors identified
4. Preventive measures proposed

**Reporting:**
1. Incident formally reported
2. Regulatory notification if required
3. Trend analysis (incident dashboard)
4. Communication to teams

**Prevention:**
1. Safety briefings/reminders issued
2. Equipment checked/repaired
3. Procedure review and update
4. Training delivered if needed
5. Announcement of safety measures

**Audit Trail:**
1. Complete documentation maintained
2. Historical incident trends tracked
3. Regulatory inspections supported
4. Insurance documentation available

**Outcomes:**
- Zero safety incidents on record
- Continuous improvement culture
- Regulatory compliance proven
- Insurance claims managed
- Personnel safety awareness

---

## Key Reports & Dashboards

### Production Dashboard

**Real-time Widgets:**

1. **My Actions Status:**
   - Overdue actions requiring immediate attention
   - Due actions for today
   - Delegated actions tracking

2. **Compliance Documents:**
   - Documents expiring in next 30 days
   - Expired documents requiring renewal
   - Documents pending approval

3. **Messages Summary:**
   - Unviewed messages count
   - Messages received today
   - Urgent messages flagged

### Production Reports

**Operational Reports:**

1. **Movements Summary Report**
   - Material volume by type
   - Movement by location
   - Movement trends by time period
   - Cost impact
   - Comparisons to forecast

2. **Shift Performance Report**
   - Shifts planned vs. actual
   - Personnel utilization
   - Equipment hours by asset
   - Productivity metrics
   - Downtime analysis

3. **Drill Bit Performance Report**
   - Metres drilled per bit type
   - Cost per metre
   - Bit lifespan vs. forecast
   - Failure analysis
   - Procurement recommendations

4. **Equipment Utilization Report**
   - Hours of operation per equipment
   - Utilization percentage
   - Maintenance events
   - Downtime root causes
   - Capacity analysis

5. **Contract Performance Report**
   - Contractor performance vs. terms
   - Cost tracking vs. contract rates
   - Milestone achievement
   - Issues and resolutions

**Financial Reports:**

1. **Cost Summary Report**
   - Costs by activity/shift/equipment
   - Variance from budget
   - Cost per unit produced
   - Contractor/vendor costs
   - Profitability analysis

2. **Invoice Tracking Report**
   - Invoices pending approval
   - Payment history
   - Vendor performance
   - Cost trends

### TeamTray Dashboard

**Real-time Widgets:**

1. **Staffing Status:**
   - Personnel on leave (current)
   - Pending leave requests
   - Upcoming roster changes
   - Critical gaps

2. **Training Status:**
   - Certifications expiring (30 days)
   - Training in progress
   - Upcoming mandatory training
   - Compliance gaps

3. **Messages Summary:**
   - Leave requests pending
   - Travel requests pending
   - Approvals required

### TeamTray Reports

**Personnel Reports:**

1. **Workforce Composition Report**
   - Personnel by role/position
   - Organizational structure
   - Tenure distribution
   - Turnover analysis

2. **Roster & Availability Report**
   - Current roster
   - Personnel availability
   - Leave planned (next 90 days)
   - Coverage by shift
   - Relief personnel identified

3. **Certification & Compliance Report**
   - Current certifications by personnel
   - Expiry timeline
   - Compliance status
   - Training required
   - Risk areas identified

4. **Training & Development Report**
   - Training completed (past period)
   - Training in progress
   - Certification completion rate
   - Development progress vs. plans
   - Skills gap analysis

5. **Activity Report Summary**
   - Reports submitted
   - Report approval rate
   - Issues identified
   - Performance trends
   - Improvement areas

**Financial Reports:**

1. **Payroll & Labor Cost Report**
   - Personnel costs by department
   - Overtime tracking
   - Cost by shift/equipment
   - Variance from budget
   - Cost per unit produced

2. **Travel & Accommodation Report**
   - Travel costs by type
   - Budget utilization
   - Vendor performance
   - Cost optimization opportunities

### Executive Dashboards

**Key Metrics Visible:**

**Production KPIs:**
- Material production vs. plan
- Equipment utilization %
- Safety incidents (YTD)
- Cost per unit produced
- Drill rate (metres/hour)

**HR KPIs:**
- Personnel availability %
- Certification compliance %
- Training completion rate
- Personnel turnover rate
- Absenteeism rate

**Financial KPIs:**
- Cost vs. budget variance
- Profitability margin
- Equipment cost per hour
- Labour cost per unit
- Overall cost per unit produced

---

## Configuration & Administration

### System Administration

**User Management:**
- Create/maintain user accounts
- Role assignment (operator, supervisor, manager)
- Permission management
- Activity audit
- Account deactivation/termination

**Data Configuration:**

**Production Configuration:**
- Equipment master data
- Material types catalog
- Activity definitions
- Location/site definitions
- KPI definitions
- Measurement units

**HR Configuration:**
- Position/role hierarchy
- Department structure
- Leave types and policies
- Training programs
- Certification types
- Travel approval limits

**Workflow Configuration:**
- Approval chains
- Notification rules
- Document requirements
- Mandatory fields
- Workflow triggers

### Audit & Compliance

**Audit Trail:**
- User login/logout tracking
- Data change history
- Approval decision tracking
- Access logs
- Report generation history

**Compliance Monitoring:**
- Certification validity tracking
- Mandatory training compliance
- Safety protocol adherence
- Regulatory requirement verification
- Documentation completeness checks

**Change Management:**
- System change log
- Configuration change history
- Data integrity checks
- Version control
- Rollback capability

---

## Summary: Business Value & Strategic Alignment

### For Production Operations Management:
- **Operational Excellence:** Complete visibility and control of all production activities
- **Cost Optimization:** Detailed cost tracking enabling continuous improvement
- **Safety Compliance:** Comprehensive documentation meets regulatory requirements
- **Asset Management:** Equipment lifecycle management maximizes value
- **Decision Support:** Real-time data enables informed decision-making

### For Human Resources:
- **Resource Planning:** Align personnel with operational requirements
- **Compliance:** Training and certification tracking ensures competency
- **Development:** Career pathways and skill tracking support retention
- **Safety:** Personnel-to-equipment link ensures qualified operators
- **Engagement:** Career progression and development opportunities improve morale

### For Senior Management:
- **Strategic Execution:** Monitor execution against business plan
- **Financial Performance:** Profitability and cost analysis by activity
- **Risk Management:** Safety, compliance, and operational risks visible
- **Capacity Planning:** Resource availability vs. demand analysis
- **Competitive Advantage:** Operational efficiency through systematic management

---

*Document prepared for Business Analyst and Stakeholder Review*  
*For detailed testing specifications, refer to TEST_CASES_SUMMARY.md*


