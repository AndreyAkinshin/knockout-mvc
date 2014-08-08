executeOnServer = function (model, url) {
  executeOnServer(model, url, null);
}
executeOnServer = function (model, url, completeFunction) {
  $.ajax({
    url: url,
    type: 'POST',
    data: ko.mapping.toJSON(model),
    dataType: "json",
    contentType: "application/json; charset=utf-8",
    success: function (data) {
      if (data.redirect) {
        location.href = resolveUrl(data.url);
      }
      else {
        ko.mapping.fromJS(data, model);
      }
    },
    error: function (error) {
      alert("There was an error posting the data to the server: " + error.responseText);
    },
    complete: completeFunction
  });

};

resolveUrl = function (url) {
  if (url.indexOf("~/") == 0) {
    url = baseUrl + url.substring(2);
  }
  return url;
};
