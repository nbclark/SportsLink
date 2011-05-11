
Function.prototype.invoke = function Function$invoke() {
    return this.apply(null, arguments);
}

Array.prototype.add = function Array$add(item) {
    this[this.length] = item;
}

Array.prototype.remove = function Array$remove(item) {
    var index = this.indexOf(item);
    if (index >= 0) {
        this.splice(index, 1);
        return true;
    }
    return false;
}

Array.prototype.removeAt = function Array$removeAt(index) {
    this.splice(index, 1);
}

Array.prototype.removeRange = function Array$removeRange(index, count) {
    return this.splice(index, count);
}

window.ss = {
    version: '0.6.1.0',

    isUndefined: function(o) {
        return (o === undefined);
    },

    isNull: function(o) {
        return (o === null);
    },

    isNullOrUndefined: function(o) {
        return (o === null) || (o === undefined);
    },

    getKeys : function(object) {
      var results = [];
      for (var property in object) {
        if (object.hasOwnProperty(property)) {
          results.push(property);
        }
      }
      return results;
    }
};

window.Type = Function;
Type.__typeName = 'Type';

window.__Namespace = function(name) {
    this.__typeName = name;
}
__Namespace.prototype = {
    __namespace: true,
    getName: function() {
        return this.__typeName;
    }
}

Type.registerNamespace = function Type$registerNamespace(name) {
    if (!window.__namespaces) {
        window.__namespaces = {};
    }
    if (!window.__rootNamespaces) {
        window.__rootNamespaces = [];
    }

    if (window.__namespaces[name]) {
        return;
    }

    var ns = window;
    var nameParts = name.split('.');

    for (var i = 0; i < nameParts.length; i++) {
        var part = nameParts[i];
        var nso = ns[part];
        if (!nso) {
            ns[part] = nso = new __Namespace(nameParts.slice(0, i + 1).join('.'));
            if (i == 0) {
                window.__rootNamespaces.add(nso);
            }
        }
        ns = nso;
    }

    window.__namespaces[name] = ns;
}

Type.prototype.registerClass = function Type$registerClass(name, baseType, interfaceType) {
    this.prototype.constructor = this;
    this.__typeName = name;
    this.__class = true;
    this.__baseType = baseType || Object;
    if (baseType) {
        this.__basePrototypePending = true;
    }

    if (interfaceType) {
        this.__interfaces = [];
        for (var i = 2; i < arguments.length; i++) {
            interfaceType = arguments[i];
            this.__interfaces.add(interfaceType);
        }
    }
}


Type.prototype.registerInterface = function Type$createInterface(name) {
    this.__typeName = name;
    this.__interface = true;
}


Type.getType = function Type$getType(typeName) {
    if (!typeName) {
        return null;
    }

    if (!Type.__typeCache) {
        Type.__typeCache = {};
    }

    var type = Type.__typeCache[typeName];
    if (!type) {
        type = eval(typeName);
        Type.__typeCache[typeName] = type;
    }
    return type;
}

Type.parse = function Type$parse(typeName) {
    return Type.getType(typeName);
}

Type.prototype.setupBase = function Type$setupBase() {
    if (this.__basePrototypePending) {
        var baseType = this.__baseType;
        if (baseType.__basePrototypePending) {
            baseType.setupBase();
        }

        for (var memberName in baseType.prototype) {
            var memberValue = baseType.prototype[memberName];
            if (!this.prototype[memberName]) {
                this.prototype[memberName] = memberValue;
            }
        }

        delete this.__basePrototypePending;
    }
}

if (!Type.prototype.resolveInheritance) {
    // This function is not used by Script#; Visual Studio relies on it
    // for JavaScript IntelliSense support of derived types.
    Type.prototype.resolveInheritance = Type.prototype.setupBase;
}

Type.prototype.initializeBase = function Type$initializeBase(instance, args) {
    if (this.__basePrototypePending) {
        this.setupBase();
    }

    if (!args) {
        this.__baseType.apply(instance);
    }
    else {
        this.__baseType.apply(instance, args);
    }
}

Type.prototype.callBaseMethod = function Type$callBaseMethod(instance, name, args) {
    var baseMethod = this.__baseType.prototype[name];
    if (!args) {
        return baseMethod.apply(instance);
    }
    else {
        return baseMethod.apply(instance, args);
    }
}

Type.prototype.get_baseType = function Type$get_baseType() {
    return this.__baseType || null;
}

Type.prototype.get_fullName = function Type$get_fullName() {
    return this.__typeName;
}

ss.listen = function Listen(target, name, onEvent, isScript)
{
    target.addEventListener(name, onEvent, false);
    if (isScript) {
        target.addEventListener("error", onEvent, false);
    }
}
ss.loadScripts = function LoadScripts(scripts, onCompleted)
{
  for (var i = 0; i < scripts.length; ++i)
  {
    var script = document.createElement('script');
    script.src = scripts[i];
    ss.listen(script, "load", onCompleted, true);

    document.getElementsByTagName("head")[0].appendChild(script);
  }
}


ss.Delegate = function Delegate$() {}

ss.Delegate.create = function Delegate$_create(object, method) {

   return function() { return method.apply(object, arguments); }
}



///////////////////////////////////////////////////////////////////////////////
// Date Extensions

Date.__typeName = 'Date';

Date.empty = null;

Date.get_now = function Date$get_now() {
    return new Date();
}
String.isNullOrEmpty = function String$isNullOrEmpty(s) {
    return !s || !s.length;
}

///////////////////////////////////////////////////////////////////////////////
// IEnumerator

ss.IEnumerator = function IEnumerator$() { };
ss.IEnumerator.prototype = {
    get_current: null,
    moveNext: null,
    reset: null
}

ss.IEnumerator.getEnumerator = function ss_IEnumerator$getEnumerator(enumerable) {
    if (enumerable) {
        return enumerable.getEnumerator ? enumerable.getEnumerator() : new ss.ArrayEnumerator(enumerable);
    }
    return null;
}

ss.IEnumerator.registerInterface('IEnumerator');

///////////////////////////////////////////////////////////////////////////////
// IEnumerable

ss.IEnumerable = function IEnumerable$() { };
ss.IEnumerable.prototype = {
    getEnumerator: null
}
ss.IEnumerable.registerInterface('IEnumerable');

///////////////////////////////////////////////////////////////////////////////
// ArrayEnumerator

ss.ArrayEnumerator = function ArrayEnumerator$(array) {
    this._array = array;
    this._index = -1;
}
ss.ArrayEnumerator.prototype = {
    get_current: function ArrayEnumerator$get_current() {
        return this._array[this._index];
    },
    moveNext: function ArrayEnumerator$moveNext() {
        this._index++;
        return (this._index < this._array.length);
    },
    reset: function ArrayEnumerator$reset() {
        this._index = -1;
    }
}

ss.ArrayEnumerator.registerClass('ArrayEnumerator', null, ss.IEnumerator);
