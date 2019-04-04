;
(function (global, $) {
    const Message = function (msgType, msgTitle, msg, func, trxId) {
        return new Message.init(msgType, msgTitle, msg, func, trxId);
    }
    Message.init = function (msgType, msgTitle, msg, func, trxId) {
        this.msgType = msgType || "";
        this.msgTitle = msgTitle || "";
        this.msg = msg || "";
        this.trxId = trxId || "";
        this.func = func || "";

        this.msgToastrType = {
            "info": "fa fa-info-circle",
            "warning": " fa-warning",
            "success": "fa fa-check-square-o",
            "error": "fa fa-times-circle",
        };
        this.txtColor = {
            "info": "blue",
            "warning": "yellow",
            "success": "green",
            "error": "red",
        };
    }
    Message.prototype = {
        getError: function () {
            var errorList = "";
            if (Array.isArray(this.msg) || typeof this.msg === "object") {
                $.each(this.msg, function (key, value) {
                    errorList += '<div>' + (key + 1) + ". " + value + '</div>';
                });
            } else {
                errorList += '<div>' + this.msg + '</div>';
            }
            this.msg = errorList;
        },
        showToastrMsg: function () {
            if (this.msgType === "error") {
                this.getError();
            }
            iziToast.show({
                title: this.msgTitle,
                message: this.msg,
                icon: this.msgToastrType[this.msgType],
                position: 'topRight',
                backgroundColor: '',
                theme: 'light', // dark
                color: this.txtColor[this.msgType], // blue, red, green, yellow
                timeout: 10000,
            });
            return this;
        },
        confirmAction: function () {
            var self = this;
            var promiseObj = new Promise(function (resolve, reject) {
                iziToast.question({
                    timeout: 20000,
                    close: false,
                    overlay: true,
                    displayMode: 'once',
                    id: 'question',
                    zindex: 9999,
                    title: 'Confirm: ',
                    message: self.msg,
                    position: 'topCenter',
                    timeout: 0,
                    buttons: [
                        ['<button>Yes</button>', function (instance, toast) {

                            instance.hide({ transitionOut: 'fadeOut' }, toast, 'button');
                            resolve(true);

                        }],
                        ['<button><b>NO</b></button>', function (instance, toast) {

                            instance.hide({ transitionOut: 'fadeOut' }, toast, 'button');
                        }, true],
                    ],
                });
            });
            return promiseObj;
        }
    }

    Message.init.prototype = Message.prototype;

    return global.Message = global.$M = Message;
}(window, $));