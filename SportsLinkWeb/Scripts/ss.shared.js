
window.ss.getKeys = function(object) {
      var results = [];
      for (var property in object) {
        if (object.hasOwnProperty(property)) {
          results.push(property);
        }
      }
      return results;
    }
