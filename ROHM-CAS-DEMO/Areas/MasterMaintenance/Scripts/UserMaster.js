(function () {

    $(document).ready(function () {
        const user = $U();
        user.populateData();
        $("#btn_add").click(function () {
            user.addUser();
        });
        $(".closeBtn").click(function () {
            user.clearUserData();
        });
        $("#btn_add_row").click(function () {
            user.addModule();
        });
        $("#frm_User").on("submit", function (e) {
            e.preventDefault();
            user.saveUser();
        });
        $('#tbl_users tbody').on('click', 'tr', function () {
            user.en_dis_ableUserRow($(this));
        });
        $('#btn_edit').click(function () {
            user.editUserData();
        });
        $('#btn_delete').click(function () {
            user.msg = "Are you sure you want to delete this user?";
            user.confirmAction().then(function (approve) {
                if (approve)
                    user.deleteUserData();
            });
        });
        $('#tblDD').on("click", ".btn-remove", function () {
            user.removeModuleSelect($(this).data("counter"));
        });
        $('#tblDD').on("change", "#modID_0, .modules", function () {
            user.getAllModules($(this));
        });
        window.Parsley.on('field:error', function () {
            $("#div-create-user > div.tabbable-custom > ul > li").removeClass("active");
            $("#div-create-user > div.tabbable-custom > ul > li:first-child").addClass("active");
            $(".tab-pane").removeClass("active");
            $("#UserInfo").addClass("active");
        });
        user.userTable.on('draw', function () {
            user.resetBtns();
        });
    })
})();