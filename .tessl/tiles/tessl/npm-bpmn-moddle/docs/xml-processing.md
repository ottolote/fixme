# XML Processing

Core functionality for parsing BPMN XML files into JavaScript objects and serializing objects back to XML format with BPMN 2.0 validation.

## Capabilities

### BpmnModdle Constructor

Creates a new BPMN Moddle instance for XML processing operations.

```javascript { .api }
/**
 * Creates a new BPMN Moddle instance
 * @param packages - Package definitions (optional, defaults to built-in BPMN packages)
 * @param options - Configuration options for the moddle instance
 */
constructor(packages?: Object, options?: Object);
```

**Usage Example:**

```javascript
import BpmnModdle from 'bpmn-moddle';

// Create with default BPMN packages
const moddle = new BpmnModdle();

// Create with custom packages
const customModdle = new BpmnModdle({
  custom: customPackageDefinition
}, {
  lax: true
});
```

### fromXML Method

Parses BPMN XML string into a JavaScript object tree using the BPMN 2.0 meta-model.

```javascript { .api }
/**
 * Instantiates a BPMN model tree from XML string
 * Supports multiple overloads for flexible parameter handling
 * @param xmlStr - The XML string to parse
 * @param typeName - Name of the root element type (default: 'bpmn:Definitions')
 * @param options - Parser options
 * @returns Promise resolving to ParseResult
 */
fromXML(xmlStr: string): Promise<ParseResult>;
fromXML(xmlStr: string, options: Object): Promise<ParseResult>;
fromXML(xmlStr: string, typeName: string, options?: Object): Promise<ParseResult>;

interface ParseResult {
  rootElement: ModdleElement;        // The parsed root element
  references: Array<Object>;         // Cross-references found during parsing
  warnings: Array<Error>;           // Non-fatal parsing warnings
  elementsById: Object;             // Map of element IDs to elements
}
```

**Usage Examples:**

```javascript
// Basic XML parsing
const xmlStr = `<?xml version="1.0" encoding="UTF-8"?>
<bpmn2:definitions xmlns:bpmn2="http://www.omg.org/spec/BPMN/20100524/MODEL"
                   id="sample-definitions"
                   targetNamespace="http://bpmn.io/schema/bpmn">
  <bpmn2:process id="Process_1">
    <bpmn2:startEvent id="StartEvent_1" />
  </bpmn2:process>
</bpmn2:definitions>`;

const { rootElement, warnings, elementsById } = await moddle.fromXML(xmlStr);

console.log(rootElement.$type); // 'bpmn:Definitions'
console.log(rootElement.get('id')); // 'sample-definitions'

// Parse with specific root type
const processXml = `<bpmn2:process xmlns:bpmn2="http://www.omg.org/spec/BPMN/20100524/MODEL" 
                                  id="Process_1">
  <bpmn2:startEvent id="StartEvent_1" />
</bpmn2:process>`;

const { rootElement: process } = await moddle.fromXML(processXml, 'bpmn:Process');
console.log(process.$type); // 'bpmn:Process'

// Parse with options
const { rootElement: definitions } = await moddle.fromXML(xmlStr, {
  lax: true  // Enable lenient parsing
});
```

### toXML Method

Serializes a BPMN object tree back to XML string format.

```javascript { .api }
/**
 * Serializes a BPMN 2.0 object tree to XML
 * @param element - The root element to serialize (typically bpmn:Definitions)
 * @param options - Serialization options
 * @returns Promise resolving to SerializationResult
 */
toXML(element: ModdleElement, options?: Object): Promise<SerializationResult>;

interface SerializationResult {
  xml: string;  // The generated XML string
}
```

**Usage Examples:**

```javascript
// Create and serialize BPMN model
const definitions = moddle.create('bpmn:Definitions', {
  id: 'Definitions_1',
  targetNamespace: 'http://bpmn.io/schema/bpmn'
});

const process = moddle.create('bpmn:Process', { id: 'Process_1' });
definitions.get('rootElements').push(process);

const { xml } = await moddle.toXML(definitions);
console.log(xml); // Generated BPMN XML

// Serialize with formatting options
const { xml: formattedXml } = await moddle.toXML(definitions, {
  format: true,
  preamble: true
});
```

### Error Handling

XML processing operations can throw ParseError for malformed or invalid BPMN XML. Non-fatal issues are returned as warnings in the ParseResult.

```javascript { .api }
interface ParseError extends Error {
  warnings: Array<Error>;  // Parsing warnings that occurred
}
```

**Usage Examples:**

```javascript
// Handle parsing errors
try {
  const result = await moddle.fromXML(invalidXmlString);
} catch (error) {
  if (error.warnings) {
    console.error('Parse warnings:', error.warnings);
  }
  console.error('Parse error:', error.message);
}

// Handle warnings in successful parsing
const { rootElement, warnings } = await moddle.fromXML(xmlString);
if (warnings.length > 0) {
  console.warn('Parse warnings:', warnings.map(w => w.message));
}

// Common warning types:
// - Unresolved references: 'unresolved reference <elementId>'
// - Unparsable content: 'unparsable content <elementName> detected'
// - Duplicate IDs: 'duplicate ID <id>'
// - Invalid attributes: 'illegal first char attribute name'
// - Unsupported encoding: 'unsupported document encoding <encoding>'
```

## Supported BPMN Elements

The XML processor supports the complete BPMN 2.0 specification including:

### Process Elements
- `bpmn:Definitions` - Root container for BPMN definitions
- `bpmn:Process` - Business process definition
- `bpmn:Collaboration` - Process collaboration and message flows

### Flow Elements
- Tasks: `bpmn:Task`, `bpmn:ServiceTask`, `bpmn:UserTask`, `bpmn:ScriptTask`
- Events: `bpmn:StartEvent`, `bpmn:EndEvent`, `bpmn:IntermediateCatchEvent`
- Gateways: `bpmn:ExclusiveGateway`, `bpmn:ParallelGateway`, `bpmn:InclusiveGateway`
- Subprocesses: `bpmn:SubProcess`, `bpmn:CallActivity`

### Diagram Elements
- `bpmndi:BPMNDiagram` - Visual diagram definition
- `bpmndi:BPMNShape` - Visual shape representations
- `bpmndi:BPMNEdge` - Visual connection representations