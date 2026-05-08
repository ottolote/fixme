# BPMN Moddle

BPMN Moddle is a JavaScript library for reading and writing BPMN 2.0 diagram files in Node.js and browsers. It provides a programmatic interface to create, modify, and parse BPMN process definitions using the BPMN 2.0 meta-model to validate input and produce correct BPMN 2.0 XML.

## Package Information

- **Package Name**: bpmn-moddle
- **Package Type**: npm
- **Language**: JavaScript/TypeScript
- **Installation**: `npm install bpmn-moddle`

## Core Imports

```javascript
import BpmnModdle from 'bpmn-moddle';
```

For CommonJS:

```javascript
const BpmnModdle = require('bpmn-moddle');
```

## Basic Usage

```javascript
import BpmnModdle from 'bpmn-moddle';

const moddle = new BpmnModdle();

const xmlStr =
  '<?xml version="1.0" encoding="UTF-8"?>' +
  '<bpmn2:definitions xmlns:bpmn2="http://www.omg.org/spec/BPMN/20100524/MODEL" ' +
                     'id="empty-definitions" ' +
                     'targetNamespace="http://bpmn.io/schema/bpmn">' +
  '</bpmn2:definitions>';

// Parse XML to object model
const { rootElement: definitions } = await moddle.fromXML(xmlStr);

// Modify the model
definitions.set('id', 'NEW_ID');

// Create new BPMN elements
const bpmnProcess = moddle.create('bpmn:Process', { id: 'MyProcess_1' });
definitions.get('rootElements').push(bpmnProcess);

// Serialize back to XML
const { xml: xmlStrUpdated } = await moddle.toXML(definitions);
```

## Architecture

BPMN Moddle is built on top of the moddle and moddle-xml frameworks, providing:

- **BPMN Meta-model**: Complete BPMN 2.0 type system with built-in validation
- **XML Processing**: Bi-directional conversion between XML and JavaScript objects
- **Object Model**: Rich JavaScript API for creating and manipulating BPMN elements
- **Extension Support**: Pluggable architecture for adding custom BPMN extensions
- **Built-in Packages**: Pre-configured with standard BPMN, BPMN DI, and drawing canvas schemas

## Capabilities

### XML Processing

Core functionality for parsing BPMN XML files into JavaScript objects and serializing objects back to XML.

```javascript { .api }
// Note: The default export is actually SimpleBpmnModdle function, used like a constructor
function BpmnModdle(packages?: Object, options?: Object): BpmnModdle;

class BpmnModdle {
  fromXML(xmlStr: string, typeName?: string, options?: Object): Promise<ParseResult>;
  toXML(element: ModdleElement, options?: Object): Promise<SerializationResult>;
  create(type: string, properties?: Object): ModdleElement;
  getType(name: string): ModdleType;
}

interface ParseResult {
  rootElement: ModdleElement;
  references: Array<Object>;
  warnings: Array<Error>;
  elementsById: Object;
}

interface SerializationResult {
  xml: string;
}
```

[XML Processing](./xml-processing.md)

### Element Creation and Manipulation

Object model functionality for creating BPMN elements and manipulating their properties.

```javascript { .api }
// Element creation
create(type: string, properties?: Object): ModdleElement;

// Create elements in undefined namespaces for custom extensions  
createAny(type: string, namespace: string, properties?: Object): ModdleElement;

// Type system access
getType(name: string): ModdleType;

// Element property access
interface ModdleElement {
  get(propertyName: string): any;
  set(propertyName: string, value: any): void;
  $instanceOf(type: string): boolean;
  $type: string;
}
```

[Element Operations](./element-operations.md)

### Package Extensions

Support for extending BPMN with custom element types and properties through package definitions.

```javascript { .api }
// Factory function with built-in packages
function SimpleBpmnModdle(additionalPackages?: Object, options?: Object): BpmnModdle;

// Built-in package schemas
interface BuiltInPackages {
  bpmn: Object;     // Core BPMN 2.0 elements
  bpmndi: Object;   // BPMN Diagram Interchange
  dc: Object;       // Drawing Canvas elements  
  di: Object;       // Diagram Interchange
  bioc: Object;     // BPMN.io custom elements
  color: Object;    // BPMN in Color extension
}
```

[Package Extensions](./package-extensions.md)

## Types

```javascript { .api }
interface ModdleElement {
  $type: string;
  $instanceOf(type: string): boolean;
  get(propertyName: string): any;
  set(propertyName: string, value: any): void;
}

interface ModdleType {
  $descriptor: TypeDescriptor;
}

interface TypeDescriptor {
  name: string;
  properties: PropertyDefinition[];
  propertiesByName: Object;
  allProperties: PropertyDefinition[];
  superClass?: string[];
}

interface PropertyDefinition {
  name: string;
  type: string;
  isMany?: boolean;
  isReference?: boolean;
}

interface ParseError extends Error {
  warnings: Array<Error>;
}
```