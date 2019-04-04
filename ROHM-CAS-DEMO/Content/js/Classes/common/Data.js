; (function () {
    const DataClass = function (formData, formAction) {
        return new DataClass.init(formData, formAction);
    }
    DataClass.init = function (formData, formAction) {
        this.formData = formData || {};
        this.formAction = formAction || "";
        this.jsonData = {};
        this.submitType = "POST";
        this.responseData = {};
        $M.init.call(this);

    }
    DataClass.prototype = {
        setJsonData: function () {
            let self = this;
            this.jsonData = {};
            $.each(self.formData, function () {
                if (self.jsonData[this.name]) {
                    if (!self.jsonData[this.name].push) {
                        self.jsonData[this.name] = [self.jsonData[this.name]];
                    }
                    self.jsonData[this.name].push(this.value || '');
                } else {
                    self.jsonData[this.name] = this.value || '';
                }
            });
            return this;
        },
        clearFromData: function (formID) {
            let self = this;
            $.each($("#" + formID + " .input"), function () {
                if ($(this).hasClass("input")) {
                    $(this).val("");
                }
            });
            $("#" + formID).parsley().reset();
            return this;
        },
        sendData: function () {
            let self = this;
            let promiseObj = new Promise(function (resolve, reject) {
                $.ajax({
                    dataType: 'json',
                    type: self.submitType,
                    url: self.formAction,
                    data: self.jsonData,
                    beforeSend: function () {
                        $('#loading_modal').modal("show");
                    },
                    success: function (response) {

                        $('#loading_modal').modal('hide');
                        if (response.success) {
                            self.responseData = response.data;
                            self.msgType = "success";
                            self.msgTitle = "Success!";
                            self.msg = response.msg;
                            resolve(true);
                        } else {
                            self.msgType = "error";
                            self.msgTitle = "Error!";
                            self.msg = response.errors;
                        }
                        if (response.hasOwnProperty('errors') || response.hasOwnProperty('msg')) {
                            self.showToastrMsg();
                        }
                    }
                });
            });
            return promiseObj;
        },
        makeSelect: function (id, data) {
            var option = '<option value="">--Please Select--</option>';
            $.each(data, function (i, x) {
                option += '<option value="' + x.value + '">' + x.text + '</option>';
            });
            if (id) {
                $('#' + id).append(option);
            } else {
                return option;
            }
        }
    }
    DataClass.init.prototype = $.extend(DataClass.prototype, $M.init.prototype);

    DataClass.init.prototype = DataClass.prototype;
    return window.DataClass = window.$D = DataClass;
}());