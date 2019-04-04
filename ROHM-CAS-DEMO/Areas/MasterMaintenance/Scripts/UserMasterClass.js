(function () {
    const User = function () {
        return new User.init();
    }
    User.init = function () {
        $D.init.call(this);
        this.editID = "";
        this.modList = {};
        this.UserModList = [];
        this.userList = {};
        this.userTable = "";
        this.counter = 0;
    }
    User.prototype = {
        drawUserTable: function () {
            if (!$.fn.DataTable.isDataTable('#tbl_users')) {
                return $('#tbl_users').DataTable({
                    processing: true,
                    serverSide: true,
                    "ajax": {
                        "url": "/MasterMaintenance/UserMaster/GetUserList",
                        "type": "POST",
                        "datatype": "json"
                    },
                    dataSrc: "data",
                    columns: [
                        { title: "User ID", data: "UserID" },
                        { title: "First Name", data: "FirstName" },
                        { title: "Middle Name", data: 'MiddleName' },
                        { title: "Last Name", data: 'LastName' },
                        {
                            title: "Access Type",
                            data: 'AccessType',
                            render: function (data, type, row, meta) {
                                switch (data) {
                                    case 1:
                                        data = "System Admin";
                                        break;
                                    case 2:
                                        data = "Admin";
                                        break;
                                    case 3:
                                        data = "Production";
                                        break;
                                    case 4:
                                        data = "Test Dev";
                                        break;
                                    default:
                                        data = "n/a";
                                        break;
                                }
                                return data;
                            }
                        },
                        { title: "Division", data: 'REPIDiv' },

                    ],
                    "createdRow": function (row, data, dataIndex) {
                        $(row).addClass('tr-data');
                        $(row).attr('data-id', data.ID);
                    }
                })
            } else {
                this.showError("Please try again.");
                return;
            }
        },
        populateData: function (formAction) {
            let self = this;
            self.formAction = formAction || "";
            if (self.formAction) {
                self.sendData().then(function () {
                    self.makeSelect("REPIDiv", self.responseData.divList)
                    self.makeSelect("modID_0", self.responseData.modList);
                    self.modList = self.responseData.modList;
                    self.userList = self.responseData.userList;
                });
            } else {
                self.showError("Please try again.");
            }
            return this;
        },
        addUser: function () {
            $("#mdl_user_input").modal("show");
            $("#mdl_user_input .modal-title").text("Add User");
            $("#btn_save").text("Save User");
            this.editID = "";
            return this;
        },
        clearUserData: function () {
            this.clearFromData("frm_User");
            $("#mdl_user_input").modal("hide");
            $("#div-create-user > div.tabbable-custom > ul > li").removeClass("active");
            $("#div-create-user > div.tabbable-custom > ul > li:first-child").addClass("active");
            $(".tab-pane").removeClass("active");
            $("#UserInfo").addClass("active");
            $(".addedd-mod-sel").remove();
            return this;
        },
        resetBtns: function () {
            this.editID = "";
            $('#btn_edit').attr("disabled", "disabled");
            $('#btn_delete').attr("disabled", "disabled");
            return this;
        },
        addModule: function () {
            this.counter++;
            var html = "<tr id='tr-" + this.counter + "' class='addedd-mod-sel'>" +
                            "<td>" +
                                "<select class='form-control input-sm input modules' id='modID_" + this.counter + "' >" +
                                    this.makeSelect("", this.modList) +
                                "</select>" +
                            "</td>" +
                            "<td class='td-action-button-container'><span type='button' data-counter='" + this.counter + "' class='fa fa-times-circle text-danger td-action-button clickable btn-remove'></span></td>" +
                        "</tr>";
            $("#tblDD tbody").append(html);
            return this;
        },
        saveUser: function (frmData) {
            let self = this;
            self.formData = frmData || "";
            if (self.formData) {

                if (self.editID) {
                    self.formAction = '/MasterMaintenance/UserMaster/UpdateUser';
                } else {
                    self.formAction = '/MasterMaintenance/UserMaster/SaveUser';
                }
                if (this.validateModules()) {
                    // self.setJsonData() -> input with no "name" attribute will not be included.
                    self.setJsonData().sendData().then(function (response) {
                        self.clearUserData().resetBtns();
                        self.userTable.ajax.reload(null, false);
                    });
                }
            } else {
                self.showError("Please try again.");
            }
            return this;
        },
        en_dis_ableUserRow: function (userRow) {
            if (userRow.hasClass('selected')) {
                userRow.removeClass('selected');
                this.editID = "";
                $('#btn_edit').attr("disabled", "disabled");
                $('#btn_delete').attr("disabled", "disabled");
            }
            else {
                this.userTable.$('tr.selected').removeClass('selected');
                userRow.addClass('selected');
                this.editID = userRow.data("id");
                $('#btn_edit').removeAttr("disabled");
                $('#btn_delete').removeAttr("disabled");
            }
            return this;
        },
        editUserData: function (formAction) {
            let self = this;
            self.formAction = formAction || "";
            if (self.formAction) {
                self.jsonData = { ID: self.editID };
                self.sendData().then(function (response) {
                    self.populateUserDetails(self.responseData.userDetails);
                });
            } else {
                self.showError("Please try again.");
            }
            return this;
        },
        populateUserDetails: function (userDetails) {
            let self = this;
            $("#ID").val(userDetails.ID);
            $("#UserID").val(userDetails.UserID);
            $("#Password").val(userDetails.Password);
            $("#FirstName").val(userDetails.FirstName);
            $("#MiddleName").val(userDetails.MiddleName);
            $("#LastName").val(userDetails.LastName);
            $("#AccessType").val(userDetails.AccessType);
            $("#REPIDiv").val(userDetails.REPIDiv);
            $("#mdl_user_input").modal("show");
            $("#mdl_user_input .modal-title").text("Update User");
            $("#btn_save").text("Update User");
            return this;
        },
        deleteUserData: function (formAction) {
            let self = this;
            self.formAction = formAction || "";
            if (self.formAction) {
                self.jsonData = { ID: this.editID };
                self.sendData().then(function (response) {
                    self.userTable.ajax.reload(null, false);
                });
            } else {
                self.showError("Please try again.");
            }
            return this;
        },
        removeModuleSelect: function (counter) {
            $("#tr-" + counter).remove();
            this.getAllModules();
            return this;
        },
        getAllModules: function (moduleSel) {
            var self = this;
            var moduleList = [$("#modID_0").val()];
            var modVal = moduleSel.val();
            var modId = moduleSel.attr("id");
            var error = false;
            if ($(".modules").length > 1) {
                $(".modules").each(function (el) {
                    var currval = $(this).val();
                    var currID = this.id;
                    if (currID !== modId) {
                        if (modVal === currval) {
                            error = true;
                        }
                    } else {
                        $('#' + currID).removeClass('input-error');
                        moduleList.push($(this).val())
                    }
                });
            }
            if (error) {
                self.showError("Duplicate values.");
                $("#" + modId).val('');
                $("#" + modId).addClass('input-error');
            }
            $("#ModuleID").val(moduleList);
            return this;
        },
        validateModules: function () {
            if (!$("#ModuleID").val()) {
                $("#div-create-user > div.tabbable-custom > ul > li").removeClass("active");
                $("#div-create-user > div.tabbable-custom > ul > li:nth-child(2)").addClass("active");
                $(".tab-pane").removeClass("active");
                $("#modules").addClass("active");
                this.showError("Please select at least one module.");
                return false;
            }
            return true;
        },
    }
    User.init.prototype = $.extend(User.prototype, $D.init.prototype);
    return window.User = window.$U = User;
})();