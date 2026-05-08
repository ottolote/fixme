# Element Operations

Object model functionality for creating BPMN elements and manipulating their properties. All BPMN elements are represented as ModdleElement objects with rich property access and type checking capabilities.

## Capabilities

### Element Creation

Create new BPMN elements using the moddle type system.

```javascript { .api }
/**
 * Creates a new BPMN element of the specified type
 * @param type - BPMN element type (e.g., 'bpmn:Process', 'bpmn:Task')
 * @param properties - Initial property values for the element
 * @returns ModdleElement instance
 */
create(type: string, properties?: Object): ModdleElement;

/**
 * Creates elements in undefined namespaces for custom extensions
 * @param type - Element type name
 * @param namespace - XML namespace URI
 * @param properties - Initial property values for the element
 * @returns ModdleElement instance
 */
createAny(type: string, namespace: string, properties?: Object): ModdleElement;
```

**Usage Examples:**

```javascript
// Create basic elements
const definitions = moddle.create('bpmn:Definitions', {
  id: 'Definitions_1',
  targetNamespace: 'http://bpmn.io/schema/bpmn'
});

const process = moddle.create('bpmn:Process', {
  id: 'Process_1',
  isExecutable: true
});

const startEvent = moddle.create('bpmn:StartEvent', {
  id: 'StartEvent_1',
  name: 'Process Started'
});

// Create complex elements with nested properties
const task = moddle.create('bpmn:Task', {
  id: 'Task_1',
  name: 'Perform Work'
});

const sequenceFlow = moddle.create('bpmn:SequenceFlow', {
  id: 'SequenceFlow_1',
  sourceRef: startEvent,
  targetRef: task
});

// Create with collection properties
const subProcess = moddle.create('bpmn:SubProcess', {
  id: 'SubProcess_1',
  flowElements: [startEvent, task, sequenceFlow]
});

// Create elements in custom namespaces
const customElement = moddle.createAny('vendor:CustomTask', 'http://vendor.com/ns', {
  id: 'CustomTask_1',
  customProperty: 'value'
});

const inputParameter = moddle.createAny('camunda:inputParameter', 'http://camunda.org/schema/1.0/bpmn', {
  name: 'myParam',
  value: 'paramValue'
});
```

### Type System Access

Access type descriptors and meta-information about BPMN element types.

```javascript { .api }
/**
 * Gets the type descriptor for a BPMN element type
 * @param name - Type name to retrieve
 * @returns ModdleType descriptor with metadata
 */
getType(name: string): ModdleType;

interface ModdleType {
  $descriptor: TypeDescriptor;
}

interface TypeDescriptor {
  propertiesByName: Object;  // Property definitions indexed by name
}
```

**Usage Examples:**

```javascript
// Get type information
const processType = moddle.getType('bpmn:Process');
console.log(processType.$descriptor); // Type metadata

// Check available properties
const taskType = moddle.getType('bpmn:Task');
const properties = taskType.$descriptor.propertiesByName;
console.log(Object.keys(properties)); // Available property names

// Validate type existence
try {
  const customType = moddle.getType('custom:MyElement');
} catch (error) {
  console.log('Type not found');
}
```

### Property Access

Access and modify element properties with type safety and validation.

```javascript { .api }
interface ModdleElement {
  $type: string;  // Element type name
  
  /**
   * Gets the value of an element property
   * @param propertyName - Name of the property to retrieve
   * @returns Property value
   */
  get(propertyName: string): any;
  
  /**
   * Sets the value of an element property
   * @param propertyName - Name of the property to set
   * @param value - Value to set
   */
  set(propertyName: string, value: any): void;
  
  /**
   * Checks if element is an instance of the specified type
   * @param type - Type name to check against
   * @returns True if element is instance of type
   */
  $instanceOf(type: string): boolean;
}
```

**Usage Examples:**

```javascript
// Property access
const process = moddle.create('bpmn:Process', { id: 'Process_1' });

// Get properties
console.log(process.get('id')); // 'Process_1'
console.log(process.get('isExecutable')); // undefined (not set)

// Set properties
process.set('isExecutable', true);
process.set('name', 'Main Process');

// Access with namespace
process.set('bpmn:isExecutable', false);

// Work with collection properties
const rootElements = definitions.get('rootElements');
rootElements.push(process);

// Direct property access (alternative syntax)
console.log(process.id); // 'Process_1'
process.name = 'Updated Process Name';
```

### Type Checking

Verify element types and inheritance relationships using the BPMN type system.

```javascript { .api }
/**
 * Checks if element is an instance of the specified type
 * @param type - Type name to check against (supports inheritance)
 * @returns True if element is instance of type or inherits from it
 */
$instanceOf(type: string): boolean;
```

**Usage Examples:**

```javascript
const task = moddle.create('bpmn:Task');
const serviceTask = moddle.create('bpmn:ServiceTask');
const gateway = moddle.create('bpmn:ExclusiveGateway');

// Direct type checking
console.log(task.$type); // 'bpmn:Task'
console.log(task.$instanceOf('bpmn:Task')); // true

// Inheritance checking
console.log(serviceTask.$instanceOf('bpmn:Task')); // true (ServiceTask extends Task)
console.log(serviceTask.$instanceOf('bpmn:Activity')); // true (Task extends Activity)
console.log(gateway.$instanceOf('bpmn:FlowNode')); // true (Gateway extends FlowNode)

// Type discrimination
if (element.$instanceOf('bpmn:Activity')) {
  console.log('Element is an activity');
} else if (element.$instanceOf('bpmn:Gateway')) {
  console.log('Element is a gateway');
} else if (element.$instanceOf('bpmn:Event')) {
  console.log('Element is an event');
}
```

### Element Relationships

Work with element references and containment relationships.

**Usage Examples:**

```javascript
// Parent-child relationships
const process = moddle.create('bpmn:Process');
const task = moddle.create('bpmn:Task', { id: 'Task_1' });

// Add to collection
const flowElements = process.get('flowElements');
flowElements.push(task);

// Element references
const sequenceFlow = moddle.create('bpmn:SequenceFlow', {
  id: 'Flow_1',
  sourceRef: task,  // Reference to source element
  targetRef: endEvent  // Reference to target element
});

// Working with incoming/outgoing flows
const incoming = task.get('incoming');
const outgoing = task.get('outgoing');
incoming.push(sequenceFlow);

// Navigate relationships
console.log(sequenceFlow.get('sourceRef').get('id')); // Access referenced element
```

## Common BPMN Element Patterns

### Process Structure

```javascript
// Create complete process structure
const definitions = moddle.create('bpmn:Definitions', {
  id: 'Definitions_1',
  targetNamespace: 'http://bpmn.io/schema/bpmn'
});

const process = moddle.create('bpmn:Process', {
  id: 'Process_1',
  isExecutable: true
});

definitions.get('rootElements').push(process);
```

### Flow Elements

```javascript
// Create flow elements with proper connections
const startEvent = moddle.create('bpmn:StartEvent', { id: 'start' });
const task = moddle.create('bpmn:Task', { id: 'task', name: 'Do Work' });
const endEvent = moddle.create('bpmn:EndEvent', { id: 'end' });

const flow1 = moddle.create('bpmn:SequenceFlow', {
  id: 'flow1',
  sourceRef: startEvent,
  targetRef: task
});

const flow2 = moddle.create('bpmn:SequenceFlow', {
  id: 'flow2', 
  sourceRef: task,
  targetRef: endEvent
});

// Add to process
const flowElements = process.get('flowElements');
flowElements.push(startEvent, task, endEvent, flow1, flow2);
```

### Default Values

Some BPMN elements have default values that are automatically set:

```javascript
// Elements with defaults
const gateway = moddle.create('bpmn:Gateway');
console.log(gateway.get('gatewayDirection')); // 'Unspecified'

const eventBasedGateway = moddle.create('bpmn:EventBasedGateway');
console.log(eventBasedGateway.get('eventGatewayType')); // 'Exclusive'

const activity = moddle.create('bpmn:Activity');
console.log(activity.get('startQuantity')); // 1
console.log(activity.get('completionQuantity')); // 1

const catchEvent = moddle.create('bpmn:CatchEvent');
console.log(catchEvent.get('parallelMultiple')); // false
```