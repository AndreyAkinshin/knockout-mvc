executeOnServer = function (model, url) {

    $.ajax({
        url: url,
        type: 'POST',
        data: ko.mapping.toJSON(model),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            ko.mapping.fromJS(data, model);
            //alert(JSON.stringify(data));
        },
        error: function (error) {
            alert("There was an error posting the data to the server: " + error.responseText);
        }
    });

};