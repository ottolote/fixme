# Package Extensions

Support for extending BPMN with custom element types and properties through package definitions. BPMN Moddle provides both a simplified factory function with built-in packages and support for custom extensions.

## Capabilities

### Default Export (SimpleBpmnModdle)

The main export of bpmn-moddle is the SimpleBpmnModdle function, which creates a BpmnModdle instance with all standard BPMN packages pre-loaded. It acts as a factory function but is used like a constructor.

```javascript { .api }
/**
 * Creates a BpmnModdle instance with built-in BPMN packages
 * The default export of 'bpmn-moddle' - used like a constructor
 * @param additionalPackages - Custom package definitions to merge with built-ins
 * @param options - Configuration options passed to BpmnModdle constructor
 * @returns BpmnModdle instance with built-in and additional packages
 */
function SimpleBpmnModdle(additionalPackages?: Object, options?: Object): BpmnModdle;
```

**Usage Examples:**

```javascript
import BpmnModdle from 'bpmn-moddle';

// Create with all built-in packages (recommended for most use cases)  
// The default export is SimpleBpmnModdle function, used like a constructor
const moddle = new BpmnModdle();

// Add custom packages to built-ins
const moddle = new BpmnModdle({
  custom: customPackageDefinition,
  vendor: vendorExtensionPackage
});

// Create with options
const moddle = new BpmnModdle({}, {
  lax: true  // Enable lenient parsing mode
});
```

### Built-in Package Schemas

Standard BPMN packages included by default in SimpleBpmnModdle.

```javascript { .api }
interface BuiltInPackages {
  bpmn: Object;     // Core BPMN 2.0 elements and attributes
  bpmndi: Object;   // BPMN Diagram Interchange for visual information
  dc: Object;       // Drawing Canvas elements (shapes, bounds)
  di: Object;       // Generic Diagram Interchange
  bioc: Object;     // BPMN.io custom elements and properties
  color: Object;    // BPMN in Color extension for color coding
}
```

**Package Details:**

- **bpmn**: Core BPMN 2.0 specification elements (`resources/bpmn/json/bpmn.json`)
  - Process elements: Process, Task, Event, Gateway, SequenceFlow
  - Data objects: DataObject, DataStore, Message
  - Collaboration: Participant, MessageFlow, Conversation
  
- **bpmndi**: BPMN Diagram Interchange (`resources/bpmn/json/bpmndi.json`)
  - Visual representations: BPMNDiagram, BPMNShape, BPMNEdge
  - Layout information: bounds, waypoints, labels
  
- **dc**: Drawing Canvas (`resources/bpmn/json/dc.json`)
  - Basic shapes: Rectangle, Circle, Polyline
  - Geometric primitives: Point, Bounds, Font
  
- **di**: Diagram Interchange (`resources/bpmn/json/di.json`)
  - Base diagram elements: Diagram, Plane, Node, Edge
  - Style definitions: color, font, line properties
  
- **bioc**: BPMN.io extensions (`resources/bpmn-io/json/bioc.json`)
  - Custom attributes for modeling tools
  - Extended properties for enhanced functionality
  
- **color**: BPMN in Color extension (external package)
  - Color coding support for BPMN elements
  - Semantic color assignments

### Custom Package Integration

Add custom BPMN extensions by providing package definitions.

```javascript { .api }
/**
 * Package definition structure for custom extensions
 */
interface PackageDefinition {
  name: string;           // Package name
  uri: string;           // XML namespace URI
  prefix: string;        // XML namespace prefix
  types: TypeDefinition[]; // Custom element types
}

interface TypeDefinition {
  name: string;          // Type name
  extends?: string;      // Parent type (optional)
  properties: PropertyDefinition[]; // Element properties
}

interface PropertyDefinition {
  name: string;          // Property name
  type: string;          // Property type
  isMany?: boolean;     // Array property flag
  isReference?: boolean; // Reference property flag
}
```

**Usage Examples:**

```javascript
// Define custom package
const customPackage = {
  name: 'Custom',
  uri: 'http://example.com/custom',
  prefix: 'custom',
  types: [
    {
      name: 'CustomTask',
      extends: 'bpmn:Task',
      properties: [
        { name: 'customAttribute', type: 'String' },
        { name: 'priority', type: 'Integer' },
        { name: 'assignedUsers', type: 'String', isMany: true }
      ]
    },
    {
      name: 'CustomEvent',
      extends: 'bpmn:StartEvent',
      properties: [
        { name: 'triggerCondition', type: 'String' },
        { name: 'relatedData', type: 'custom:DataReference', isReference: true }
      ]
    }
  ]
};

// Use custom package
const moddle = new BpmnModdle({
  custom: customPackage
});

// Create custom elements
const customTask = moddle.create('custom:CustomTask', {
  id: 'CustomTask_1',
  name: 'Custom Work',
  customAttribute: 'special-value',
  priority: 5,
  assignedUsers: ['user1', 'user2']
});

// Verify custom type
console.log(customTask.$instanceOf('bpmn:Task')); // true
console.log(customTask.$instanceOf('custom:CustomTask')); // true
```

### Loading External Extensions

Load pre-defined extension packages from external modules.

**Usage Examples:**

```javascript
// Load Camunda BPM extension
import camundaModdleDescriptor from 'camunda-bpmn-moddle/resources/camunda.json';

const moddle = new BpmnModdle({
  camunda: camundaModdleDescriptor
});

// Create Camunda-specific elements
const serviceTask = moddle.create('bpmn:ServiceTask', {
  id: 'ServiceTask_1',
  'camunda:delegateExpression': '${myDelegate}'
});

// Load Zeebe extension
import zeebeModdleDescriptor from 'zeebe-bpmn-moddle/resources/zeebe.json';

const zeebeModdle = new BpmnModdle({
  zeebe: zeebeModdleDescriptor
});

// Use Zeebe properties
const zeebeTask = zeebeModdle.create('bpmn:ServiceTask', {
  id: 'ServiceTask_1',
  'zeebe:taskDefinition': {
    type: 'my-worker'
  }
});
```

### Package Validation and Error Handling

Handle package loading errors and validation issues.

**Usage Examples:**

```javascript
try {
  const moddle = new BpmnModdle({
    invalid: malformedPackageDefinition
  });
} catch (error) {
  console.error('Package loading failed:', error.message);
}

// Validate custom elements during creation
try {
  const element = moddle.create('custom:UnknownType');
} catch (error) {
  console.error('Unknown element type:', error.message);
}

// Check package availability
const processType = moddle.getType('bpmn:Process');
if (processType) {
  console.log('BPMN package loaded successfully');
}

try {
  const customType = moddle.getType('custom:MyType');
  console.log('Custom package available');
} catch (error) {
  console.log('Custom package not loaded');
}
```

### Resource Access

Access package resource files directly for advanced use cases. Note that ES modules require import attributes for JSON files.

```javascript { .api }
// Package exports provide access to resource files (ES modules with import attributes)
import BpmnPackage from 'bpmn-moddle/resources/bpmn/json/bpmn.json' with { type: 'json' };
import BpmnDiPackage from 'bpmn-moddle/resources/bpmn/json/bpmndi.json' with { type: 'json' };
import DcPackage from 'bpmn-moddle/resources/bpmn/json/dc.json' with { type: 'json' };
import DiPackage from 'bpmn-moddle/resources/bpmn/json/di.json' with { type: 'json' };
import BiocPackage from 'bpmn-moddle/resources/bpmn-io/json/bioc.json' with { type: 'json' };
```

**Usage Examples:**

```javascript
// Access raw package definitions (ES modules)
import BpmnPackage from 'bpmn-moddle/resources/bpmn/json/bpmn.json' with { type: 'json' };

console.log(BpmnPackage.name); // 'BPMN20'
console.log(BpmnPackage.types.length); // Number of defined types

// Create moddle with specific packages only
// Note: Using raw BpmnModdle class constructor directly (not the default SimpleBpmnModdle factory)
import BpmnModdle from 'bpmn-moddle/lib/bpmn-moddle.js';

const minimalModdle = new BpmnModdle({
  bpmn: BpmnPackage  // Only core BPMN, no diagram interchange
});

// Inspect package structure
const taskType = BpmnPackage.types.find(t => t.name === 'Task');
console.log(taskType.properties); // Task properties definition
```

## Extension Development Best Practices

### Namespace Management

```javascript
// Use descriptive namespaces
const myExtension = {
  name: 'MyCompany',
  uri: 'http://mycompany.com/bpmn/extensions/v1',
  prefix: 'myco',
  // ...
};

// Avoid conflicts with existing prefixes
// Reserved: bpmn, bpmndi, dc, di, bioc, color
```

### Type Inheritance

```javascript
// Extend appropriate base types
const customTask = {
  name: 'MyCustomTask',
  extends: 'bpmn:Task',  // Inherits Task behavior
  properties: [
    // Add custom properties only
  ]
};

// For completely new concepts, extend base classes
const customArtifact = {
  name: 'MyArtifact', 
  extends: 'bpmn:Artifact',  // Base for non-flow elements
  properties: [
    // Define artifact-specific properties
  ]
};
```