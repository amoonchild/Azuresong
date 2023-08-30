"use strict";

function _readOnlyError(name) { throw new TypeError("\"" + name + "\" is read-only"); }
function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }
// mergeInto(LibraryManager.library, );

// GetExternalJS: function() {
//   var script = document.createElement("script");
//   script.src = "https://scriptURLhere";
//   document.head.appendChild(script);
// }

var ImSDKPlugin = {
  $tim: {},
  $store: {
    friendList: null,
    friendApplicationList: null,
    groupList: null,
    blackList: null,
    loginUser: null
  },
  $utils: {
    _handleNotLoggedIn: function _handleNotLoggedIn() {
      console.warn('Please Init First!');
    },
    _handleNoTIM: function _handleNoTIM() {
      console.warn('Please import TIM Web SDK! Check on https://cloud.tencent.com/document/product/269/75285');
    },
    _handleNotImplemented: function _handleNotImplemented() {
      console.warn('Not support for the web');
    },
    getStrBuf: function getStrBuf(str) {
      if (typeof str !== 'string') {
        str = JSON.stringify(str);
      }
      var len = lengthBytesUTF8(str) + 1;
      var buf = _malloc(len);
      stringToUTF8(str, buf, len);
      return buf;
    },
    getFileBlobUsingURL: function getFileBlobUsingURL(url, convertBlob) {
      var xhr = new XMLHttpRequest();
      xhr.open("GET", url);
      xhr.responseType = "blob";
      xhr.addEventListener('load', function () {
        convertBlob(xhr.response);
      });
      xhr.send();
    },
    blobToFile: function blobToFile(blob, name) {
      var file = new File([blob], name);
      return file;
    },
    getFileObjectFromURL: function getFileObjectFromURL(filePathOrUrl, convertBlob) {
      var _this = this;
      var fileName = filePathOrUrl.split('/').pop();
      this.getFileBlobUsingURL(filePathOrUrl, function (blob) {
        convertBlob(_this.blobToFile(blob, fileName));
      });
    },
    isInit: false
  },
  $converter: {
    // Generate Js From C#
    // from----------------------------------------------------------------
    fromFriendPendencyType: function fromFriendPendencyType(type) {
      var list = [TIM.TYPES.SNS_APPLICATION_SENT_TO_ME, TIM.TYPES.SNS_APPLICATION_SENT_BY_ME, TIM.TYPES.SNS_APPLICATION_TYPE_BOTH];
      return list[type];
    },
    fromFriendType: function fromFriendType(type) {
      var list = [TIM.TYPES.SNS_ADD_TYPE_SINGLE, TIM.TYPES.SNS_ADD_TYPE_BOTH];
      return list[type];
    },
    fromConvType: function fromConvType(type) {
      var convType = ['', TIM.TYPES.CONV_C2C, TIM.TYPES.CONV_GROUP, TIM.TYPES.CONV_SYSTEM];
      return convType[type];
    },
    fromGender: function fromGender(type) {
      var genderType = [TIM.TYPES.GENDER_UNKNOWN, TIM.TYPES.GENDER_MALE, TIM.TYPES.GENDER_FEMALE];
      return genderType[type];
    },
    fromAddPermission: function fromAddPermission(type) {
      var addType = ['', TIM.TYPES.ALLOW_TYPE_ALLOW_ANY, TIM.TYPES.ALLOW_TYPE_NEED_CONFIRM, TIM.TYPES.ALLOW_TYPE_DENY_ANY];
      return addType[type];
    },
    fromPriority: function fromPriority(priority) {
      var priorityList = [TIM.TYPES.MSG_PRIORITY_HIGH, TIM.TYPES.MSG_PRIORITY_NORMAL, TIM.TYPES.MSG_PRIORITY_LOW, TIM.TYPES.MSG_PRIORITY_LOWEST];
      return priorityList[priority];
    },
    fromReceiveMsgOpt: function fromReceiveMsgOpt(opt) {
      var optList = [TIM.TYPES.MSG_REMIND_ACPT_AND_NOTE, TIM.TYPES.MSG_REMIND_DISCARD, TIM.TYPES.MSG_REMIND_ACPT_NOT_NOTE];
      return optList[opt];
    },
    fromGroupType: function fromGroupType(type) {
      var optList = [TIM.TYPES.GRP_PUBLIC, TIM.TYPES.GRP_WORK, TIM.TYPES.GRP_MEETING, TIM.TYPES.GRP_AVCHATROOM, TIM.TYPES.GRP_AVCHATROOM, TIM.TYPES.GRP_COMMUNITY];
      return optList[type];
    },
    fromRole: function fromRole(role) {
      switch (role) {
        case 400:
          return TIM.TYPES.GRP_MBR_ROLE_OWNER;
        case 300:
          return TIM.TYPES.GRP_MBR_ROLE_ADMIN;
        default:
          return TIM.TYPES.GRP_MBR_ROLE_MEMBER;
      }
    },
    fromJoinOption: function fromJoinOption(type) {
      var optList = [TIM.TYPES.JOIN_OPTIONS_DISABLE_APPLY, TIM.TYPES.JOIN_OPTIONS_NEED_PERMISSION, TIM.TYPES.JOIN_OPTIONS_FREE_ACCESS];
      return optList[type];
    },
    fromGroupAddOption: function fromGroupAddOption(type) {
      var optList = [TIM.TYPES.JOIN_OPTIONS_DISABLE_APPLY, TIM.TYPES.JOIN_OPTIONS_NEED_PERMISSION, TIM.TYPES.JOIN_OPTIONS_FREE_ACCESS];
      return optList[type];
    },
    fromCreateGroup: function fromCreateGroup(param) {
      var _this2 = this;
      var res = {};
      res.name = param.create_group_param_group_name;
      res.type = this.fromGroupType(param.create_group_param_group_type);
      res.groupID = param.create_group_param_group_id;
      res.introduction = param.create_group_param_introduction;
      res.notification = param.create_group_param_notification;
      res.avatar = param.create_group_param_face_url;
      res.maxMemberNum = param.create_group_param_max_member_num;
      res.joinOption = this.fromJoinOption(param.create_group_param_add_option);
      if (param.create_group_param_group_member_array) {
        res.memberList = param.create_group_param_group_member_array.map(function (member) {
          var user = {};
          user.userID = member.group_member_info_identifier;
          user.role = _this2.fromRole(member.group_member_info_member_role);
          if (member.group_member_info_custom_info) {
            user.memberCustomField = member.group_member_info_custom_info.map(function (kv) {
              return {
                key: kv.key,
                value: kv.value
              };
            });
          }
          return user;
        });
      }
      return res;
    },
    fromGroupInfoCustemString: function fromGroupInfoCustemString(list) {
      if (!list) {
        return null;
      }
      return list.map(function (custom) {
        return {
          key: custom.group_info_custom_string_info_key,
          value: custom.group_info_custom_string_info_value
        };
      });
    },
    fromGroupModifyInfoParam: function fromGroupModifyInfoParam(param) {
      var res = {};
      res.groupID = param.group_modify_info_param_group_id;
      if (param.group_modify_info_param_group_name) {
        res.name = param.group_modify_info_param_group_name;
      }
      if (param.group_modify_info_param_face_url) {
        res.avatar = param.group_modify_info_param_face_url;
      }
      if (param.group_modify_info_param_introduction) {
        res.introduction = param.group_modify_info_param_introduction;
      }
      if (param.group_modify_info_param_notification) {
        res.notification = param.group_modify_info_param_notification;
      }
      if (typeof param.group_modify_info_param_is_shutup_all === 'boolean') {
        res.muteAllMembers = param.group_modify_info_param_is_shutup_all;
      }
      if (typeof param.group_modify_info_param_add_option === 'number') {
        res.joinOption = this.fromGroupAddOption(param.group_modify_info_param_add_option);
      }
      if (param.group_modify_info_param_custom_info) {
        res.groupCustomField = this.fromGroupInfoCustemString(param.group_modify_info_param_custom_info);
      }
      return res;
    },
    // Js To C#
    // to----------------------------------------------------------------
    toMessageReceipt: function toMessageReceipt(message) {
      var res = {
        msg_receipt_conv_id: message.conversationID,
        msg_receipt_conv_type: this.toConvType(message.conversationType),
        msg_receipt_time_stamp: message.time,
        msg_receipt_msg_id: message.ID,
        msg_receipt_is_peer_read: message.isPeerRead,
        msg_receipt_read_count: message.readReceiptInfo.readCount,
        msg_receipt_unread_count: message.readReceiptInfo.unreadCount
      };
      return res;
    },
    toFriendProfileItem: function toFriendProfileItem(friend) {
      var res = {};
      res.friend_profile_item_remark = friend.remark;
      res.friend_profile_item_group_name_array = friend.groupList;
      if (friend.friendCustomFriend) {
        res.friend_profile_item_custom_string_array = friend.friendCustomFriend.map(function (cf) {
          return {
            friend_profile_custom_string_info_key: cf.key,
            friend_profile_custom_string_info_value: cf.value
          };
        });
      }
      return res;
    },
    toNetworkStatus: function toNetworkStatus(status) {
      switch (status) {
        case TIM.TYPES.NET_STATE_CONNECTED:
          return 0;
        case TIM.TYPES.NET_STATE_DISCONNECTED:
          return 1;
        case TIM.TYPES.NET_STATE_CONNECTING:
          return 2;
        default:
          return 3;
      }
    },
    toMsgLocator: function toMsgLocator(msg) {
      var res = {};
      res.message_locator_conv_id = msg.conversationID;
      res.message_locator_conv_type = this.toConvType(msg.conversationType);
      res.message_locator_is_revoked = msg.isRevoked;
      res.message_locator_time = msg.time;
      res.message_locator_is_self = true;
      res.message_locator_unique_id = msg.ID;
      return res;
    },
    toFriendPendencyType: function toFriendPendencyType(type) {
      switch (type) {
        case TIM.TYPES.SNS_APPLICATION_SENT_TO_ME:
          return 0;
        case TIM.TYPES.SNS_APPLICATION_SENT_BY_ME:
          return 1;
        default:
          return 2;
      }
    },
    toFriendAddPendencyInfo: function toFriendAddPendencyInfo(application) {
      var res = {};
      res.friend_add_pendency_info_type = this.toFriendPendencyType(application.type);
      res.friend_add_pendency_info_idenitifer = application.userID;
      res.friend_add_pendency_info_nick_name = application.nick;
      res.friend_add_pendency_info_add_source = application.source;
      res.friend_add_pendency_info_add_wording = application.wording;
      res.friend_add_pendency_info_add_time = application.time;
      return res;
    },
    toCheckRelation: function toCheckRelation(relation) {
      switch (relation) {
        case TIM.TYPES.SNS_TYPE_BOTH_WAY:
          return 3;
        case TIM.TYPES.SNS_TYPE_B_WITH_A:
          return 2;
        case TIM.TYPES.SNS_TYPE_A_WITH_B:
          return 1;
        default:
          return 0;
      }
    },
    toFriendProfile: function toFriendProfile(friend) {
      var res = {};
      res.friend_profile_identifier = friend.userID;
      res.friend_profile_group_name_array = friend.groupList;
      res.friend_profile_remark = friend.remark;
      res.friend_profile_add_wording = friend.wording;
      res.friend_profile_add_source = friend.source;
      res.friend_profile_user_profile = this.toUserProfile(friend.profile);
      if (friend.friendCustomFriend) {
        res.friend_profile_custom_string_array = friend.friendCustomFriend.map(function (cf) {
          return {
            friend_profile_custom_string_info_key: cf.key,
            friend_profile_custom_string_info_value: cf.value
          };
        });
      }
      return res;
    },
    toConvType: function toConvType(type) {
      switch (type) {
        case TIM.TYPES.CONV_C2C:
          return 1;
        case TIM.TYPES.CONV_GROUP:
          return 2;
        case TIM.TYPES.CONV_SYSTEM:
          return 3;
        default:
          return 0;
      }
    },
    toPriority: function toPriority(priority) {
      switch (priority) {
        case TIM.TYPES.MSG_PRIORITY_HIGH:
          return 0;
        case TIM.TYPES.MSG_PRIORITY_NORMAL:
          return 1;
        case TIM.TYPES.MSG_PRIORITY_LOW:
          return 2;
        case TIM.TYPES.MSG_PRIORITY_LOWEST:
          return 3;
      }
    },
    toUserStatusType: function toUserStatusType(status) {
      switch (status) {
        case TIM.TYPES.USER_STATUS_UNLOGINED:
          return 3;
        case TIM.TYPES.USER_STATUS_OFFLINE:
          return 2;
        case TIM.TYPES.USER_STATUS_ONLINE:
          return 1;
        default:
          return 0;
      }
    },
    toLastMessage: function toLastMessage(msg, convID, convType) {
      var res = {};
      res.message_elem_array = [this.toElement(msg)];
      if (convID) {
        res.message_conv_id = convID;
      }
      if (convType !== undefined) {
        res.message_conv_type = convType;
      }
      res.message_sender = msg.fromAccount;
      res.message_server_time = msg.lastTime;
      res.message_is_peer_read = msg.isPeerRead;
      res.message_seq = msg.lastSequence;
      res.message_is_online_msg = msg.onlineOnlyFlag;
      if (msg.cloudCustomData) {
        res.message_cloud_custom_str = msg.cloudCustomData;
      }
      return res;
    },
    toGroupAtInfoArray: function toGroupAtInfoArray(list) {
      if (!list) {
        return null;
      }
      return list.map(function (atInfo) {
        return {
          conv_group_at_info_seq: atInfo.messageSequence,
          conv_group_at_info_at_type: atInfo.atTypeArray[0]
        };
      });
    },
    toGroupType: function toGroupType(type) {
      switch (type) {
        case TIM.TYPES.GRP_PUBLIC:
          return 0;
        case TIM.TYPES.GRP_WORK:
          return 1;
        case TIM.TYPES.GRP_MEETING:
          return 2;
        case TIM.TYPES.GRP_AVCHATROOM:
          return 4;
        case TIM.TYPES.GRP_COMMUNITY:
          return 5;
        default:
          return 0;
      }
    },
    toMsgFlag: function toMsgFlag(type) {
      switch (type) {
        case TIM.TYPES.MSG_REMIND_ACPT_AND_NOTE:
          return 0;
        case TIM.TYPES.MSG_REMIND_DISCARD:
          return 1;
        case TIM.TYPES.MSG_REMIND_ACPT_NOT_NOTE:
          return 2;
      }
    },
    toRole: function toRole(type) {
      switch (type) {
        case TIM.TYPES.GRP_MBR_ROLE_ADMIN:
          return 300;
        case TIM.TYPES.GRP_MBR_ROLE_MEMBER:
          return 200;
        case TIM.TYPES.GRP_MBR_ROLE_OWNER:
          return 400;
        default:
          return 0;
      }
    },
    toGroupAddOption: function toGroupAddOption(type) {
      switch (type) {
        case TIM.TYPES.JOIN_OPTIONS_FREE_ACCESS:
          return 2;
        case TIM.TYPES.JOIN_OPTIONS_NEED_PERMISSION:
          return 1;
        case TIM.TYPES.JOIN_OPTIONS_DISABLE_APPLY:
        default:
          return 0;
      }
    },
    toGender: function toGender(gender) {
      switch (gender) {
        case TIM.TYPES.GENDER_FEMALE:
          return 2;
        case TIM.TYPES.GENDER_MALE:
          return 1;
        default:
          return 0;
      }
    },
    toAddPermission: function toAddPermission(type) {
      switch (type) {
        case TIM.TYPES.ALLOW_TYPE_DENY_ANY:
          return 3;
        case TIM.TYPES.ALLOW_TYPE_NEED_CONFIRM:
          return 2;
        case TIM.TYPES.ALLOW_TYPE_ALLOW_ANY:
          return 1;
        default:
          return 0;
      }
    },
    toGroupSelfInfo: function toGroupSelfInfo(self) {
      var res = {};
      res.group_self_info_join_time = self.joinTime;
      res.group_self_info_role = this.toRole(self.role);
      res.group_self_info_msg_flag = this.toMsgFlag(self.messageRemindType);
      return res;
    },
    toGroupBaseInfo: function toGroupBaseInfo(group) {
      var res = {};
      res.group_base_info_group_id = group.groupID;
      res.group_base_info_group_name = group.name;
      res.group_base_info_group_type = this.toGroupType(group.type);
      res.group_base_info_face_url = group.avatar;
      res.group_base_info_info_seq = Number(group.infoSequence);
      res.group_base_info_lastest_seq = Number(group.nextMessageSeq);
      if (group.selfInfo) {
        res.group_base_info_readed_seq = group.selfInfo.readedSequence;
        res.group_base_info_msg_flag = this.toMsgFlag(group.selfInfo.messageRemindType);
        res.group_base_info_self_info = this.toGroupSelfInfo(group.selfInfo);
      }
      res.group_base_info_is_shutup_all = group.muteAllMembers;
      return res;
    },
    toConvShowName: function toConvShowName(convInfo) {
      if (convInfo.remark) {
        return convInfo.remark;
      }
      if (convInfo.groupProfile) {
        return convInfo.groupProfile.name;
      }
      if (convInfo.userProfile) {
        return convInfo.userProfile.nickname ? convInfo.userProfile.nickname : convInfo.userProfile.userID;
      }
      return '';
    },
    toConvInfo: function toConvInfo(convInfo) {
      var res = {};
      res.conv_id = convInfo.conversationID;
      res.conv_type = this.toConvType(convInfo.type);
      res.conv_unread_num = convInfo.unreadCount;
      res.conv_is_has_lastmsg = convInfo.lastMessage && convInfo.lastMessage.lastTime > 0;
      if (res.conv_is_has_lastmsg) {
        res.conv_last_msg = this.toLastMessage(convInfo.lastMessage, convInfo.conversationID);
      }
      // Draft is not supported
      res.conv_is_has_draft = false;
      res.conv_group_at_info_array = this.toGroupAtInfoArray(convInfo.groupAtInfoList);
      res.conv_is_pinned = convInfo.isPinned;
      res.conv_show_name = this.toConvShowName(convInfo);
      return res;
    },
    toGroupCustomFields: function toGroupCustomFields(groupCustomFields) {
      if (!groupCustomFields) {
        return [];
      }
      // TODO: group_tips_group_change_info_flag
      return groupCustomFields.map(function (field) {
        return {
          group_tips_group_change_info_key: field.key,
          group_tips_group_change_info_value: field.value
        };
      });
    },
    toGroupMemberInfo: function toGroupMemberInfo(member, groupID) {
      var res = {};
      res.group_member_info_identifier = member.userID;
      res.group_member_info_group_id = groupID;
      res.group_member_info_join_time = member.joinTime;
      res.group_member_info_member_role = this.toRole(member.role);
      res.group_member_info_shutup_time = member.muteUntil;
      res.group_member_info_name_card = member.nameCard;
      res.group_member_info_nick_name = member.nick;
      res.group_member_info_face_url = member.avatar;
      res.group_member_info_custom_info = this.toGroupMemberInfoCustemString(member.memberCustomField);
      return res;
    },
    toGroupMemberInfoCustemString: function toGroupMemberInfoCustemString(list) {
      if (!list) {
        return null;
      }
      return list.map(function (custom) {
        return {
          group_member_info_custom_string_info_key: custom.key,
          group_member_info_custom_string_info_value: custom.value
        };
      });
    },
    toGroupInfoCustemString: function toGroupInfoCustemString(list) {
      if (!list) {
        return null;
      }
      return list.map(function (custom) {
        return {
          group_info_custom_string_info_key: custom.key,
          group_info_custom_string_info_value: custom.value
        };
      });
    },
    toGroupTopicInfo: function toGroupTopicInfo(topic) {
      var res = {};
      res.group_topic_info_topic_id = topic.topicID;
      res.group_topic_info_topic_name = topic.topicName;
      res.group_topic_info_introduction = topic.introduction;
      res.group_topic_info_notification = topic.notification;
      res.group_topic_info_topic_face_url = topic.avatar;
      res.group_topic_info_is_all_muted = topic.muteAllMembers;
      if (topic.selfInfo) {
        res.group_topic_info_self_mute_time = topic.selfInfo.muteTime ? topic.selfInfo.muteTime : 0;
      }
      res.group_topic_info_custom_string = topic.customData;
      res.group_topic_info_unread_count = topic.unreadCount;
      res.group_topic_info_last_message = this.toMessage(topic.lastMessage);
      res.group_topic_info_group_at_info_array = this.toGroupAtInfoArray(topic.groupAtInfoList);
      return res;
    },
    toGroupDetailInfo: function toGroupDetailInfo(group) {
      var res = {};
      res.group_detial_info_group_id = group.groupID;
      res.group_detial_info_group_type = this.toGroupType(group.type);
      res.group_detial_info_is_support_topic = group.isSupportTopic;
      res.group_detial_info_group_name = group.name;
      res.group_detial_info_notification = group.notification;
      res.group_detial_info_introduction = group.introduction;
      res.group_detial_info_face_url = group.avatar;
      res.group_detial_info_create_time = group.createTime;
      res.group_detial_info_info_seq = group.infoSequence;
      res.group_detial_info_last_info_time = group.lastInfoTime;
      res.group_detial_info_next_msg_seq = group.nextMessageSeq;
      if (group.lastMessage) {
        res.group_detial_info_last_msg_time = group.lastMessage.lastTime;
      }
      res.group_detial_info_member_num = group.memberCount;
      res.group_detial_info_max_member_num = group.maxMemberCount;
      res.group_detial_info_add_option = this.toGroupAddOption(group.joinOption);
      res.group_detial_info_is_shutup_all = group.muteAllMembers;
      res.group_detial_info_owener_identifier = group.ownerID;
      if (group.groupCustomField) {
        var list = this.toGroupInfoCustemString(group.groupCustomField);
        res.create_group_param_custom_info = list;
      }
      return res;
    },
    toUserProfile: function toUserProfile(profile) {
      var res = {};
      res.user_profile_identifier = profile.userID;
      res.user_profile_nick_name = profile.nick;
      res.user_profile_gender = this.toGender(profile.gender);
      res.user_profile_face_url = profile.avatar;
      res.user_profile_self_signature = profile.selfSignature;
      res.user_profile_add_permission = this.toAddPermission(profile.allowType);
      res.user_profile_location = profile.location;
      res.user_profile_language = profile.language;
      res.user_profile_birthday = profile.birthday;
      res.user_profile_level = profile.level;
      res.user_profile_role = profile.role;
      if (profile.profileCustomField) {
        res.user_profile_custom_string_array = profile.profileCustomField.map(function (cf) {
          return {
            user_profile_custom_string_info_key: cf.key,
            user_profile_custom_string_info_value: cf.value
          };
        });
      }
      return res;
    },
    toMemberList: function toMemberList(memberList) {
      return memberList.map(function (member) {
        return {
          group_tips_member_change_info_identifier: member.userID,
          group_tips_member_change_info_shutupTime: member.muteTime
        };
      });
    },
    toMergeMessages: function toMergeMessages(mergeMessages) {
      var _this3 = this;
      if (!mergeMessages) {
        return null;
      }
      return mergeMessages.map(function (simplifiedMessage) {
        var res = {};
        res.message_msg_id = simplifiedMessage.ID;
        res.message_sender = simplifiedMessage.from;
        if (simplifiedMessage.messageBody) {
          res.message_elem_array = simplifiedMessage.messageBody.map(function (msg) {
            return _this3.toElement(msg);
          });
        }
        res.message_server_time = simplifiedMessage.time;
        if (simplifiedMessage.cloudCustomData) {
          res.message_cloud_custom_str = simplifiedMessage.cloudCustomData;
        }
        return res;
      });
    },
    toElement: function toElement(msg) {
      var elem = {};
      var payload = msg.payload;
      switch (msg.type) {
        case TIM.TYPES.MSG_TEXT:
          {
            elem.elem_type = 0;
            elem.text_elem_content = payload.text;
            break;
          }
        case TIM.TYPES.MSG_IMAGE:
          {
            elem.elem_type = 1;
            elem.image_elem_format = payload.imageFormat;
            elem.image_elem_orig_path = payload.uuid;
            var imgArr = payload.imageInfoArray;
            for (var i = 0; i < imgArr.length; i++) {
              var img = imgArr[i];
              if (imgArr[i].type === 0) {
                // Original Image
                elem.image_elem_level = 0;
                elem.image_elem_orig_pic_height = img.height;
                elem.image_elem_orig_pic_width = img.width;
                elem.image_elem_orig_pic_size = img.size;
                elem.image_elem_orig_url = img.url;
              } else if (imgArr[i].type === 1) {
                // Small Image
                elem.image_elem_level = 1;
                elem.image_elem_thumb_pic_height = img.height;
                elem.image_elem_thumb_pic_width = img.width;
                elem.image_elem_thumb_pic_size = img.size;
                elem.image_elem_thumb_url = img.url;
              } else if (imgArr[i].type === 2) {
                // Big Image
                elem.image_elem_level = 2;
                elem.image_elem_large_pic_height = img.height;
                elem.image_elem_large_pic_width = img.width;
                elem.image_elem_large_pic_size = img.size;
                elem.image_elem_large_url = img.url;
              }
            }
            break;
          }
        case TIM.TYPES.MSG_AUDIO:
          {
            elem.elem_type = 2;
            elem.sound_elem_file_path = payload.uuid;
            elem.sound_elem_file_size = payload.size;
            elem.sound_elem_file_time = payload.second;
            elem.sound_elem_url = payload.url;
            break;
          }
        case TIM.TYPES.MSG_CUSTOM:
          {
            elem.elem_type = 3;
            elem.custom_elem_data = payload.data;
            elem.custom_elem_desc = payload.description;
            elem.custom_elem_ext = payload.extension;
            break;
          }
        case TIM.TYPES.MSG_FILE:
          {
            elem.elem_type = 4;
            elem.file_elem_file_path = payload.uuid;
            elem.file_elem_file_name = payload.fileName;
            elem.file_elem_file_size = payload.fileSize;
            elem.file_elem_url = payload.fileUrl;
            break;
          }
        case TIM.TYPES.MSG_GRP_TIP:
          {
            elem.elem_type = 5;
            elem.group_tips_elem_tip_type = payload.operationType;
            elem.group_tips_elem_op_user = payload.operatorID;
            if (payload.newGroupProfile) {
              elem.group_tips_elem_group_change_info_array = this.toGroupCustomFields(payload.newGroupProfile.groupCustomField);
            }
            if (payload.memberList) {
              elem.group_tips_elem_member_change_info_array = this.toMemberList(payload.memberList);
            }
            break;
          }
        case TIM.TYPES.MSG_FACE:
          {
            elem.elem_type = 6;
            elem.face_elem_index = payload.index;
            elem.face_elem_buf = payload.data;
            break;
          }
        case TIM.TYPES.MSG_LOCATION:
          {
            elem.elem_type = 7;
            elem.location_elem_desc = payload.description;
            elem.location_elem_longitude = payload.longitude;
            elem.location_elem_latitude = payload.latitude;
            break;
          }
        case TIM.TYPES.MSG_GRP_SYS_NOTICE:
          {
            elem.elem_type = 8;
            break;
          }
        case TIM.TYPES.MSG_VIDEO:
          {
            elem.elem_type = 9;
            elem.video_elem_video_type = payload.videoFormat;
            elem.video_elem_video_size = payload.videoSize;
            elem.video_elem_video_duration = payload.videoSecond;
            elem.video_elem_video_path = payload.videoUUID;
            elem.video_elem_video_url = payload.videoUrl;
            elem.video_elem_image_width = payload.snapshotWidth ? payload.snapshotWidth : payload.thumbWidth;
            elem.video_elem_image_height = payload.snapshotHeight ? payload.snapshotHeight : payload.thumbHeight;
            elem.video_elem_image_url = payload.snapshotUrl ? payload.snapshotUrl : payload.thumbUrl;
            break;
          }
        case TIM.TYPES.MSG_MERGER:
          {
            elem.elem_type = 12;
            elem.merge_elem_title = payload.title;
            elem.merge_elem_abstract_array = payload.abstractList;
            elem.merge_elem_message_array = this.toMergeMessages(payload.messageList);
            elem.merge_elem_compatible_text = payload.compatibleText;
            elem.merge_elem_layer_over_limit = payload.layersOverLimit;
            elem.merge_elem_relay_json_key = payload.downloadKey;
            break;
          }
        default:
          {
            elem.elem_type = -1;
          }
      }
      return elem;
    },
    fromElement: function fromElement(elem) {
      // TOOD now it's only for modifyMessage
      var payload = {};
      switch (elem.elem_type) {
        case 0:
          {
            // Text
            payload.text = elem.text_elem_content;
            break;
          }
        case 3:
          {
            // Custom
            payload.data = elem.custom_elem_data;
            payload.description = elem.custom_elem_desc;
            payload.extension = elem.custom_elem_ext;
            break;
          }
        case 6:
          {
            // Face
            payload.index = elem.face_elem_index;
            payload.data = elem.face_elem_buf;
            break;
          }
        case 7:
          {
            // Location
            payload.description = elem.location_elem_desc;
            payload.longitude = elem.location_elem_longitude;
            payload.latitude = elem.location_elem_latitude;
          }
      }
      return payload;
    },
    toMessage: function toMessage(msg) {
      if (!msg) return null;
      var res = {};
      res.message_msg_id = msg.ID;
      res.message_elem_array = [this.toElement(msg)];
      res.message_conv_type = this.toConvType(msg.conversationType);
      res.message_conv_id = msg.conversationID;
      res.message_sender = msg.from;
      res.message_server_time = msg.time;
      if (msg.status === 'success') {
        res.message_status = 2;
      }
      if (msg.status === 'fail') {
        res.message_status = 3;
      }
      res.message_priority = this.toPriority(msg.priority);
      res.message_is_peer_read = msg.isPeerRead;
      res.message_group_at_user_array = msg.atUserList;
      res.message_cloud_custom_str = msg.cloudCustomData;
      res.message_need_read_receipt = msg.needReadReceipt;
      if (store.loginUser) {
        res.message_is_from_self = msg.from === store.loginUser;
      }
      return res;
    },
    fromMsgToSimplifiedMsg: function (_fromMsgToSimplifiedMsg) {
      function fromMsgToSimplifiedMsg(_x) {
        return _fromMsgToSimplifiedMsg.apply(this, arguments);
      }
      fromMsgToSimplifiedMsg.toString = function () {
        return _fromMsgToSimplifiedMsg.toString();
      };
      return fromMsgToSimplifiedMsg;
    }(function (msg) {
      var res = {};
      res.ID = msg.message_msg_id;
      res.conversationID = msg.message_conv_id;
      res.conversationType = this.fromConvType(msg.message_conv_type);
      res.to = msg.message_sender;
      res.from = msg.message_sender;
      res.time = msg.message_server_time;
      var bodyArr = [];
      for (var i = 0; i < msg.message_elem_array.length; i++) {
        var elem = msg.message_elem_array[i];
        var body = {};
        if (elem.elem_type === 0) {
          // Text
          body.type = TIM.TYPES.MSG_TEXT;
          body.payload = {
            text: elem.text_elem_content
          };
        } else if (elem.elem_type === 1) {
          // Image
          body.type = TIM.TYPES.MSG_IMAGE;
          body.payload = {
            uuid: elem.image_elem_orig_path,
            imageFormat: elem.image_elem_format,
            imageInfoArray: []
          };
          if (elem.image_elem_orig_url) {
            // Original Image
            var img = {};
            img.type = 0;
            img.height = elem.image_elem_orig_pic_height;
            img.width = elem.image_elem_orig_pic_width;
            img.size = elem.image_elem_orig_pic_size;
            img.url = elem.image_elem_orig_url;
            body.payload.imageInfoArray.push(img);
          }
          if (elem.image_elem_thumb_url) {
            // Small Image
            var _img = {};
            _img.type = 1;
            _img.height = elem.image_elem_thumb_pic_height;
            _img.width = elem.image_elem_thumb_pic_width;
            _img.size = elem.image_elem_thumb_pic_size;
            _img.url = elem.image_elem_thumb_url;
            body.payload.imageInfoArray.push(_img);
          }
          if (elem.image_elem_large_url) {
            // Big Image
            var _img2 = {};
            _img2.type = 2;
            _img2.height = elem.image_elem_large_pic_height;
            _img2.width = elem.image_elem_large_pic_width;
            _img2.size = elem.image_elem_large_pic_size;
            _img2.url = elem.image_elem_large_url;
            body.payload.imageInfoArray.push(_img2);
          }
        } else if (elem.elem_type === 2) {
          // Sound
          body.type = TIM.TYPES.MSG_AUDIO;
          body.payload = {
            uuid: elem.sound_elem_file_path,
            url: elem.sound_elem_url,
            size: elem.sound_elem_file_size,
            second: elem.sound_elem_file_time
          };
        } else if (elem.elem_type === 3) {
          // Custom
          body.type = TIM.TYPES.MSG_CUSTOM;
          body.payload = {
            data: elem.custom_elem_data,
            description: elem.custom_elem_desc,
            extension: elem.custom_elem_ext
          };
        } else if (elem.elem_type === 4) {
          // File
          body.type = TIM.TYPES.MSG_FILE;
          body.payload = {
            uuid: elem.file_elem_file_path,
            fileName: elem.file_elem_file_name,
            fileUrl: elem.file_elem_url,
            fileSize: elem.file_elem_file_size
          };
        } else if (elem.elem_type === 6) {
          // Face
          body.type = TIM.TYPES.MSG_FACE;
          body.payload = {
            index: elem.face_elem_index,
            data: elem.face_elem_buf
          };
        } else if (elem.elem_type === 7) {
          // Location
          body.type = TIM.TYPES.MSG_LOCATION;
          body.payload = {
            description: elem.location_elem_desc,
            longitude: elem.location_elem_longitude,
            latitude: elem.location_elem_latitude
          };
        } else if (elem.elem_type === 9) {
          // Video
          body.type = TIM.TYPES.MSG_VIDEO;
          body.payload = {
            videoFormat: elem.video_elem_video_type,
            videoSecond: elem.video_elem_video_duration,
            videoSize: elem.video_elem_video_size,
            videoUrl: elem.video_elem_video_url,
            videoUUID: elem.video_elem_video_path,
            snapshotWidth: elem.video_elem_image_width,
            snapshotHeight: elem.video_elem_image_height,
            snapshotUrl: elem.video_elem_image_url
          };
        } else if (elem.elem_type === 12) {
          // Merger
          body.type = TIM.TYPES.MSG_MERGER;
          body.payload = {
            downloadKey: elem.merge_elem_relay_json_key,
            messageList: elem.merge_elem_message_array.map(function (msg) {
              return fromMsgToSimplifiedMsg(msg);
            }),
            title: elem.merge_elem_title,
            abstractList: elem.merge_elem_abstract_array,
            compatibleText: elem.merge_elem_compatible_text,
            layersOverLimit: elem.merge_elem_layer_over_limit
          };
        }
        if (body.type) {
          bodyArr.push(body);
        }
      }
      res.messageBody = bodyArr;
      return res;
    })
  },
  $callback: {
    onSdkReady: null,
    onSdkNotReady: null,
    onMessageReceived: null,
    onMessageModified: null,
    onMessageRevoked: null,
    onMessageReadByPeer: null,
    onMessageReadReceiptReceived: null,
    onConversationListUpdated: null,
    onTotalUnreadMessageCountUpdated: null,
    onConversationGroupListUpdated: null,
    onConversationInGroupUpdated: null,
    onGroupListUpdated: null,
    onGroupAttributesUpdated: null,
    onTopicCreated: null,
    onTopicDeleted: null,
    onTopicUpdated: null,
    onProfileUpdated: function onProfileUpdated(event) {
      if (callback._onSelfInfoUpdated) {
        callback._onSelfInfoUpdated(event);
      }
      if (callback._onFriendProfileUpdated) {
        callback._onFriendProfileUpdated(event);
      }
    },
    _onSelfInfoUpdated: null,
    _onFriendProfileUpdated: null,
    onUserStatusUpdated: null,
    onBlacklistUpdated: function onBlacklistUpdated(event) {
      if (!callback._onFriendBlackListAdded && !callback._onFriendBlackListDeleted) {
        return;
      }
      if (!utils.isInit) {
        utils._handleNotLoggedIn();
        return;
      }
      var oldBlackList = store.blackList ? store.blackList : [];
      var newBlackList = event.data ? event.data : [];
      var addUserIDs = [];
      var delUserIDs = [];
      var oldBlackUserIDSet = new Set(oldBlackList.map(function (black) {
        return black.userID;
      }));
      var newBlackUserIDSet = new Set(newBlackList.map(function (black) {
        return black.userID;
      }));
      // Add
      if (callback._onFriendBlackListAdded) {
        newBlackList.forEach(function (newBlack) {
          if (!oldBlackUserIDSet.has(newBlack)) {
            addUserIDs.push({
              friend_profile_identifier: newBlack
            });
          }
        });
        if (addUserIDs.length > 0) {
          callback._onFriendBlackListAdded(addUserIDs);
        }
      }
      // Del
      if (callback._onFriendBlackListDeleted) {
        oldBlackList.forEach(function (oldBlack) {
          if (!newBlackUserIDSet.has(oldBlack)) {
            delUserIDs.push(oldBlack);
          }
        });
        if (delUserIDs.length > 0) {
          callback._onFriendBlackListDeleted(delUserIDs);
        }
      }
      var promise = tim.getBlacklist();
      promise.then(function (imResponse) {
        store.blackList = imResponse.data;
      }).catch(function (imError) {
        console.warn('getBlacklist error:', imError); 
      });
    },

    _onFriendBlackListAdded: null,
    _onFriendBlackListDeleted: null,
    _onAddFriend: null,
    _onDeleteFriend: null,
    onFriendListUpdated: function onFriendListUpdated(event) {
      if (!callback._onAddFriend && !callback._onDeleteFriend) {
        return;
      }
      if (!utils.isInit) {
        utils._handleNotLoggedIn();
        return;
      }
      var oldFriendList = store.friendList ? store.friendList : []; 
      var newFriendList = event.data ? event.data : [];
      var addUserIDs = [];
      var delUserIDs = [];
      var oldFriendUserIDSet = new Set(oldFriendList.map(function (friend) {
        return friend.userID;
      }));
      var newFriendUserIDSet = new Set(newFriendList.map(function (friend) {
        return friend.userID;
      }));
      // Add
      if (callback._onAddFriend) {
        newFriendList.forEach(function (newFriend) {
          if (!oldFriendUserIDSet.has(newFriend.userID)) {
            addUserIDs.push(newFriend.userID);
          }
        });
        if (addUserIDs.length > 0) {
          callback._onAddFriend(addUserIDs);
        }
      }
      // Del
      if (callback._onDeleteFriend) {
        oldFriendList.forEach(function (oldFriend) {
          if (!newFriendUserIDSet.has(oldFriend.userID)) {
            delUserIDs.push(oldFriend.userID);
          }
        });
        if (delUserIDs.length > 0) {
          callback._onDeleteFriend(delUserIDs);
        }
      }
      var promise = tim.getFriendList();
      promise.then(function (imResponse) {
        store.friendList = imResponse.data;
      }).catch(function (imError) {
        console.warn('getFriendList error:', imError);
      });
    },

    onFriendGroupListUpdated: null,
    onFriendApplicationListUpdated: function onFriendApplicationListUpdated(event) {
      if (!callback._onFriendAddRequest && !callback._onFriendApplicationListDeleted) {
        return;
      }
      if (!utils.isInit) {
        utils._handleNotLoggedIn();
        return;
      }
      // Reference from Flutter for Web
      var oldFriendApplicationList = store.friendApplicationList ? store.friendApplicationList : [];
      var newFriendApplicationList = event.data.friendApplicationList ? event.data.friendApplicationList : [];
      var addFriendAddPendencyList = [];
      var delUserIDs = [];
      var oldFriendUserIDSet = new Set(oldFriendApplicationList.map(function (friend) {
        return friend.userID;
      }));
      var newFriendUserIDSet = new Set(newFriendApplicationList.map(function (friend) {
        return friend.userID;
      }));
      // Add
      if (callback._onFriendAddRequest) {
        newFriendApplicationList.forEach(function (newFriend) {
          if (!oldFriendUserIDSet.has(newFriend.userID)) {
            addFriendAddPendencyList.push(converter.toFriendAddPendencyInfo(newFriend));
          }
        });
        if (addFriendAddPendencyList.length > 0) {
          callback._onFriendAddRequest(addFriendAddPendencyList);
        }
      }
      // Del
      if (callback._onFriendApplicationListDeleted) {
        oldFriendApplicationList.forEach(function (oldFriend) {
          if (!newFriendUserIDSet.has(oldFriend.userID)) {
            delUserIDs.push(oldFriend.userID);
          }
        });
        if (delUserIDs.length > 0) {
          callback._onFriendApplicationListDeleted(delUserIDs);
        }
      }
      var promise = tim.getFriendApplicationList();
      promise.then(function (imResponse) {
        store.friendApplicationList = imResponse.data.friendApplicationList;
      }).catch(function (imError) {
        console.warn('getFriendApplicationList error:', imError);
      });
    },

    _onFriendAddRequest: null,
    _onFriendApplicationListDeleted: null,
    onKicked: function onKicked(event) {
      if (callback._onKickedOut) {
        callback._onKickedOut(event);
      }
      if (callback._onSigExpired) {
        callback._onSigExpired(event);
      }
    },
    // Below two events in Unity are same in Web
    _onKickedOut: null,
    _onSigExpired: null,
    onError: null,
    onNetStateChange: null,
    _onMsgElemUploadProgressCallback: null
  },
  CopyText: function CopyText(strPtr) {
    var str = UTF8ToString(strPtr);
    if (navigator && navigator.clipboard && navigator.clipboard.writeText) {
      navigator.clipboard.writeText(str);
    } else {
      console.warn('Not support clipboard');
    }
  },
  TIMInit: function TIMInit(sdk_app_id, json_sdk_config) {
    if (TIM && TIM.create) {
      tim = TIM.create({
        SDKAppID: sdk_app_id
      });
      if (TIMUploadPlugin) {
        tim.registerPlugin({
          'tim-upload-plugin': TIMUploadPlugin
        });
      }
      utils.isInit = true;
      var onSdkReady = function onSdkReady(event) {
        var promise1 = tim.getFriendList();
        promise1.then(function (imResponse) {
          store.friendList = imResponse.data;
        }).catch(function (imError) {
          console.warn('getFriendList error:', imError);
        });

        var promise2 = tim.getFriendApplicationList();
        promise2.then(function (imResponse) {
          store.friendApplicationList = imResponse.data.friendApplicationList;
        }).catch(function (imError) {
          console.warn('getFriendApplicationList error:', imError);
        });

        var promise3 = tim.getBlacklist();
        promise3.then(function (imResponse) {
          store.blackList = imResponse.data;
        }).catch(function (imError) {
          console.warn('getBlacklist error:', imError);
        });
      };

      tim.on(TIM.EVENT.SDK_READY, onSdkReady);
    } else {
      utils._handleNoTIM();
    }
  },
  TIMLogin: function TIMLogin(user_id, user_sig, cb, user_data) {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    var userID = UTF8ToString(user_id);
    store.loginUser = userID;
    var promise = tim.login({
      userID: userID,
      userSig: UTF8ToString(user_sig)
    });
    promise.then(function (imResponse) {
      console.log(imResponse.data);
      var descBuf = utils.getStrBuf(imResponse.data.errorInfo);
      Module['dynCall_viiii'](cb, imResponse.data.errorCode, descBuf, descBuf, user_data);
    }).catch(function (imError) {
      console.warn('login error:', imError);
    });
  },

  TIMUninit: function TIMUninit() {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    tim.destroy();
    utils.isInit = false;
  },
  TIMGetSDKVersion: function TIMGetSDKVersion() {
    if (TIM && TIM.VERSION) {
      var ver = utils.getStrBuf(TIM.VERSION);
      return ver;
    } else {
      utils._handleNoTIM();
    }
  },
  TIMSetConfig: function TIMSetConfig() {
    utils._handleNotImplemented();
  },
  TIMGetServerTime: function TIMGetServerTime() {
    utils._handleNotImplemented();
  },
  TIMLogout: function TIMLogout(cb, user_data) {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    var promise = tim.logout();
    promise.then(function (imResponse) {
      console.log(imResponse.data);
      store.loginUser = null;
      Module['dynCall_viiii'](cb, imResponse.data.errorCode, '', '', user_data);
    }).catch(function (imError) {
      console.warn('logout error:', imError);
    });
  },
  TIMGetLoginStatus: function TIMGetLoginStatus() {
    utils._handleNotImplemented();
  },
  TIMGetLoginUserID: function TIMGetLoginUserID(user_id) {
    utils._handleNotImplemented();
  },
  TIMGetLoginUserIDWeb: function TIMGetLoginUserIDWeb(cb, user_data) {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    var promise = tim.getMyProfile();
    promise.then(function (imResponse) {
      console.log(imResponse.data);
      var userID = utils.getStrBuf(imResponse.data.userID);
      Module['dynCall_viiii'](cb, imResponse.code, '', userID, user_data);
    }).catch(function (imError) {
      console.warn('getMyProfile error:', e);
    });
  },

  TIMConvGetConvList: function TIMConvGetConvList(cb, user_data) {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    var promise = tim.getConversationList();
    promise.then(function (imResponse) {
      var convList = imResponse.data.conversationList.map(function (conv) {
        return converter.toConvInfo(conv);
      });
      console.log(convList);
      var conversationList = utils.getStrBuf(convList);
      Module['dynCall_viiii'](cb, imResponse.code, '', conversationList, user_data);
    }).catch(function (imError) {
      console.warn('getConversationList error:', imError);
    });
  },

  TIMConvDelete: function TIMConvDelete(conv_id, conv_type, cb, user_data) {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    var promise = tim.deleteConversation(UTF8ToString(conv_id));
    promise.then(function (imResponse) {
      var conversationID = imResponse.data.conversationID; 
      console.log(imResponse.data);
      Module['dynCall_viiii'](cb, imResponse.code, '', '', user_data);
    }).catch(function (imError) {
      console.warn('deleteConversation error:', imError);
    });
  },

  TIMConvGetConvInfo: function TIMConvGetConvInfo(json_get_conv_list_param, cb, user_data) {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    var get_conv_list_param = JSON.parse(UTF8ToString(json_get_conv_list_param));
    var convIDs = get_conv_list_param.map(function (param) {
      return param.get_conversation_list_param_conv_id;
    });
    var promise = tim.getConversationList(convIDs);
    promise.then(function (imResponse) {
      var convList = imResponse.data.conversationList.map(function (conv) {
        return converter.toConvInfo(conv);
      });
      console.log(convList);
      var conversationList = utils.getStrBuf(convList);
      Module['dynCall_viiii'](cb, imResponse.code, '', conversationList, user_data);
    }).catch(function (imError) {
      console.warn('getConversationList error:', imError);
    });
  },

  TIMConvSetDraft: function TIMConvSetDraft() {
    utils._handleNotImplemented();
  },
  TIMConvCancelDraft: function TIMConvCancelDraft() {
    utils._handleNotImplemented();
  },
  TIMConvPinConversation: function TIMConvPinConversation(conv_id, conv_type, is_pinned, cb, user_data) {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    var promise = tim.pinConversation({
      conversationID: UTF8ToString(conv_id),
      isPinned: Boolean(is_pinned)
    });
    promise.then(function (imResponse) {
      var conversationID = imResponse.data.conversationID;
      Module['dynCall_viiii'](cb, imResponse.code, '', '', user_data);
    }).catch(function (imError) {
      console.warn('pinConversation error:', imError);
    });
  },
  TIMConvGetTotalUnreadMessageCount: function TIMConvGetTotalUnreadMessageCount(cb,user_data){
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    let totalUnreadCount = tim.getTotalUnreadMessageCount();
    var res = {
        conv_get_total_unread_message_count_result_unread_count: totalUnreadCount
      };
    Module['dynCall_viiii'](cb, 0, '', res, user_data);
  },
  TIMConvGetUnreadMessageCountByFilter:function TIMConvGetUnreadMessageCountByFilter (){
    utils._handleNotImplemented();
  },
  TIMConvSubscribeUnreadMessageCountByFilter: function TIMConvSubscribeUnreadMessageCountByFilter(){
    utils._handleNotImplemented();
  },
  TIMConvUnsubscribeUnreadMessageCountByFilter: function TIMConvUnsubscribeUnreadMessageCountByFilter(){
    utils._handleNotImplemented();
  },
  TIMGroupDecreaseGroupCounter: function TIMGroupDecreaseGroupCounter(){
    utils._handleNotImplemented();
  },
  TIMGroupGetGroupCounters: function TIMGroupGetGroupCounters(){
    utils._handleNotImplemented();
  },
  TIMGroupIncreaseGroupCounter: function TIMGroupIncreaseGroupCounter(){
    utils._handleNotImplemented();
  },
  TIMGroupSetGroupCounters: function TIMGroupSetGroupCounters(){
    utils._handleNotImplemented();
  },
  TIMMsgTranslateText: function TIMMsgTranslateText(){
    utils._handleNotImplemented();
  },
  TIMSetConvUnreadMessageCountChangedByFilterCallback: function TIMSetConvUnreadMessageCountChangedByFilterCallback(){
    utils._handleNotImplemented();
  },
  TIMSetGroupCounterChangedCallback: function TIMSetGroupCounterChangedCallback(){
    utils._handleNotImplemented();
  },
 
  TIMConvAddConversationsToGroup: function TIMConvAddConversationsToGroup() {
    utils._handleNotImplemented();
  },
  TIMConvAddConversationsToGroup: function TIMConvAddConversationsToGroup() {
    utils._handleNotImplemented();
  },
  TIMConvCreateConversationGroup: function TIMConvCreateConversationGroup() {
    utils._handleNotImplemented();
  },
  TIMConvDeleteConversationGroup: function TIMConvDeleteConversationGroup() {
    utils._handleNotImplemented();
  },
  TIMConvDeleteConversationsFromGroup: function TIMConvDeleteConversationsFromGroup() {
    utils._handleNotImplemented();
  },
  TIMConvGetConversationGroupList: function TIMConvGetConversationGroupList() {
    utils._handleNotImplemented();
  },
  TIMConvGetConversationListByFilter: function TIMConvGetConversationListByFilter() {
    utils._handleNotImplemented();
  },
  TIMConvMarkConversation: function TIMConvMarkConversation() {
    utils._handleNotImplemented();
  },
  TIMConvRenameConversationGroup: function TIMConvRenameConversationGroup() {
    utils._handleNotImplemented();
  },
  TIMConvSetConversationCustomData: function TIMConvSetConversationCustomData() {
    utils._handleNotImplemented();
  },
  TIMMsgSendMessage: function TIMMsgSendMessage(conv_id, conv_type, message_param, message_id, cb, user_data) {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    var convType = converter.fromConvType(conv_type);
    var convID = UTF8ToString(conv_id);
    var messageParam = JSON.parse(UTF8ToString(message_param));
    var sendingOption = {};
    if (messageParam.message_is_online_msg) {
      sendingOption.onlineUserOnly = true;
    }
    if (messageParam.message_excluded_from_last_message) {
      sendingOption.messageControlInfo = {
        excludedFromLastMessage: true
      };
    }
    if (messageParam.message_is_excluded_from_unread_count) {
      if (sendingOption.messageControlInfo) {
        sendingOption.messageControlInfo.excludedFromUnreadCount = true;
      } else {
        sendingOption.messageControlInfo = {
          excludedFromUnreadCount: true
        };
      }
    }
    if (messageParam.message_offlie_push_config) {
      var config = messageParam.message_offlie_push_config;
      var offlinePushInfo = {};
      offlinePushInfo.disablePush = config.offline_push_config_flag !== 0;
      offlinePushInfo.description = config.offline_push_config_desc;
      offlinePushInfo.extension = config.offline_push_config_ext;
      if (config.offline_push_config_ios_config) {
        offlinePushInfo.title = config.offline_push_config_ios_config.ios_offline_push_config_title;
        offlinePushInfo.ignoreIOSBadge = Boolean(config.offline_push_config_ios_config.ios_offline_push_config_ignore_badge);
      }
      if (config.offline_push_config_android_config) {
        offlinePushInfo.title = config.offline_push_config_android_config.android_offline_push_config_title;
        if (config.offline_push_config_android_config.android_offline_push_config_oppo_channel_id) {
          offlinePushInfo.androidOPPOChannelID = config.offline_push_config_android_config.android_offline_push_config_oppo_channel_id;
        }
      }
      sendingOption.offlinePushInfo = offlinePushInfo;
    }
    var elem = messageParam.message_elem_array[0];
    var message;
    var msgOption = {
      to: convID,
      conversationType: convType,
      payload: {}
    };
    if (messageParam.message_need_read_receipt) {
      msgOption.needReadReceipt = true;
    }
    if (messageParam.message_priority !== undefined) {
      msgOption.priority = converter.fromPriority(messageParam.message_priority);
    }
    switch (elem.elem_type) {
      case 0:
        {
          // Text
          msgOption.payload.text = elem.text_elem_content;
          if (messageParam.message_target_group_member_array) {
            msgOption.receiverList = messageParam.message_target_group_member_array;
          }
          message = tim.createTextMessage(msgOption);
          var promise = tim.sendMessage(message, sendingOption);
          promise.then(function (imResponse) {
            console.log(imResponse);
            var msgStr = converter.toMessage(imResponse.data.message);
            var msg = utils.getStrBuf(msgStr);
            Module['dynCall_viiii'](cb, imResponse.code, '', msg, user_data);
          }).catch(function (imError) {
            console.warn('sendMessage error:', imError);
          });
          return;
        }
      case 1:
        {
          // Image
          // TODO progress
          utils.getFileObjectFromURL(elem.image_elem_orig_path, function (fileObject) {
            msgOption.payload = {
              file: fileObject
            };
            message = tim.createImageMessage(msgOption);
            // Send message
            var promise = tim.sendMessage(message, sendingOption);
            promise.then(function (imResponse) {
              console.log(imResponse);
              var msgStr = JSON.stringify(converter.toMessage(imResponse.data.message));
              var msg = utils.getStrBuf(msgStr);
              Module['dynCall_viiii'](cb, imResponse.code, '', msg, user_data);
            }).catch(function (imError) {
              console.warn('sendMessage error:', imError);
            });
          });
          return;
        }
      case 2:
        {
          // Sound
          utils.getFileObjectFromURL(elem.sound_elem_file_path, function (fileObject) {
            msgOption.payload = {
              file: fileObject
            };
            message = tim.createAudioMessage(msgOption);
            // Send message
            var promise = tim.sendMessage(message, sendingOption);
            promise.then(function (imResponse) {
              console.log(imResponse);
              var msgStr = JSON.stringify(converter.toMessage(imResponse.data.message));
              var msg = utils.getStrBuf(msgStr);
              Module['dynCall_viiii'](cb, imResponse.code, '', msg, user_data);
            }).catch(function (imError) {
              console.warn('sendMessage error:', imError);
            });
          });
          return;
        }
      case 3:
        {
          // Custom
          msgOption.payload.data = elem.custom_elem_data;
          msgOption.payload.description = elem.custom_elem_desc;
          msgOption.payload.extension = elem.custom_elem_ext;
          message = tim.createCustomMessage(msgOption);
          var _promise = tim.sendMessage(message, sendingOption);
          _promise.then(function (imResponse) {
            console.log(imResponse);
            var msgStr = JSON.stringify(converter.toMessage(imResponse.data.message));
            var msg = utils.getStrBuf(msgStr);
            Module['dynCall_viiii'](cb, imResponse.code, '', msg, user_data);
          }).catch(function (imError) {
            console.warn('sendMessage error:', imError);
          });
          return;
        }
      case 4:
        {
          // File
          // TODO progress
          utils.getFileObjectFromURL(elem.file_elem_file_path, function (fileObject) {
            msgOption.payload = {
              file: fileObject
            };
            message = tim.createFileMessage(msgOption);
            // Send message
            var promise = tim.sendMessage(message, sendingOption);
            promise.then(function (imResponse) {
              console.log(imResponse);
              var msgStr = JSON.stringify(converter.toMessage(imResponse.data.message));
              var msg = utils.getStrBuf(msgStr);
              Module['dynCall_viiii'](cb, imResponse.code, '', msg, user_data);
            }).catch(function (imError) {
              console.warn('sendMessage error:', imError);
            });
          });
          return;
        }
      case 6:
        {
          // Face
          msgOption.payload.index = elem.face_elem_index;
          msgOption.payload.data = elem.face_elem_buf;
          message = tim.createFaceMessage(msgOption);
          var _promise2 = tim.sendMessage(message, sendingOption);
          _promise2.then(function (imResponse) {
            console.log(imResponse);
            var msgStr = JSON.stringify(converter.toMessage(imResponse.data.message));
            var msg = utils.getStrBuf(msgStr);
            Module['dynCall_viiii'](cb, imResponse.code, '', msg, user_data);
          }).catch(function (imError) {
            console.warn('sendMessage error:', imError);
          });
          return;
        }
      case 7:
        {
          // Location
          msgOption.payload.description = elem.location_elem_desc;
          msgOption.payload.longitude = elem.location_elem_longitude;
          msgOption.payload.latitude = elem.location_elem_latitude;
          message = tim.createLocationMessage(msgOption);
          var _promise3 = tim.sendMessage(message, sendingOption);
          _promise3.then(function (imResponse) {
            console.log(imResponse);
            var msgStr = JSON.stringify(converter.toMessage(imResponse.data.message));
            var msg = utils.getStrBuf(msgStr);
            Module['dynCall_viiii'](cb, imResponse.code, '', msg, user_data);
          }).catch(function (imError) {
            console.warn('sendMessage error:', imError);
          });
          return;
        }
      case 9:
        {
          // Video
          utils.getFileObjectFromURL(elem.video_elem_video_path, function (fileObject) {
            msgOption.payload = {
              file: fileObject
            };
            message = tim.createVideoMessage(msgOption);
            // Send message
            var promise = tim.sendMessage(message, sendingOption);
            promise.then(function (imResponse) {
              console.log(imResponse);
              var msgStr = JSON.stringify(converter.toMessage(imResponse.data.message));
              var msg = utils.getStrBuf(msgStr);
              Module['dynCall_viiii'](cb, imResponse.code, '', msg, user_data);
            }).catch(function (imError) {
              console.warn('sendMessage error:', imError);
            });
          });
          return;
        }
      case 12:
        {
          // Merge
          msgOption.payload.messageList = elem.merge_elem_message_array.map(function (msg) {
            return converter.fromMsgToSimplifiedMsg(msg);
          });
          msgOption.payload.title = elem.merge_elem_title;
          msgOption.payload.abstractList = elem.merge_elem_abstract_array;
          msgOption.payload.compatibleText = elem.merge_elem_compatible_text;
          message = tim.createMergerMessage(msgOption);
          var _promise4 = tim.sendMessage(message, sendingOption);
          _promise4.then(function (imResponse) {
            console.log(imResponse);
            var msgStr = JSON.stringify(converter.toMessage(imResponse.data.message));
            var msg = utils.getStrBuf(msgStr);
            Module['dynCall_viiii'](cb, imResponse.code, '', msg, user_data);
          }).catch(function (imError) {
            console.warn('sendMessage error:', imError);
          });
          return;
        }
    }
  },
  TIMMsgCancelSend: function TIMMsgCancelSend() {
    utils._handleNotImplemented();
  },
  TIMMsgFindMessages: function TIMMsgFindMessages(message_id_array, cb, user_data) {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    // TODO search messageID List
    var messageID = JSON.parse(UTF8ToString(message_id_array))[0];
    var message = tim.findMessage(messageID);
    var msgStr = JSON.stringify(converter.toMessage(message));
    var msg = utils.getStrBuf(msgStr);
    Module['dynCall_viiii'](cb, 0, '', msg, user_data);
  },
  TIMMsgReportReaded: function TIMMsgReportReaded(conv_id, conv_type, message_param, cb, user_data) {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    var convType = converter.fromConvType(conv_type);
    var convID = UTF8ToString(conv_id);
    if (!convID.startsWith(convType)) {
      convID = convType + convID;
    }
    var msg = JSON.parse(UTF8ToString(message_param));
    if (!msg) {
      var _messageList = null;
      tim.getMessageList({
        conversationID: convID
      }).then(function (imResponse) {
        _messageList = imResponse.data.messageList;
        _messageList = _messageList.filter(function (msg) {
          return msg.needReadReceipt && msg.flow === 'in';
        });
        if (_messageList.length < 1) {
          Module['dynCall_viiii'](cb, 0, '', '', user_data);
          return;
        }
        tim.sendMessageReadReceipt(_messageList).then(function () {
          Module['dynCall_viiii'](cb, 0, '', '', user_data);
        }).catch(function (imError) {
          console.warn('sendMessageReadReceipt error:', imError);
        });
      });
      return;
    }
    var messageList = [];
    messageList.push(tim.findMessage(msg.message_msg_id));
    tim.sendMessageReadReceipt(messageList).then(function () {
      Module['dynCall_viiii'](cb, 0, '', '', user_data);
    }).catch(function (imError) {
      console.warn('sendMessageReadReceipt error:', imError);
    });
  },
  TIMMsgSetOfflinePushToken: function TIMMsgSetOfflinePushToken() {
    utils._handleNotImplemented();
  },
  TIMMsgDoBackground: function TIMMsgDoBackground() {
    utils._handleNotImplemented();
  },
  TIMMsgDoForeground: function TIMMsgDoForeground() {
    utils._handleNotImplemented();
  },
  TIMMsgMarkAllMessageAsRead: function TIMMsgMarkAllMessageAsRead(cb, user_data) {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    var promise = tim.setAllMessageRead();
    promise.then(function (imResponse) {
      Module['dynCall_viiii'](cb, imResponse.code, '', '', user_data);
    }).catch(function (imError) {
      console.warn('setAllMessageRead error:', imError);
    });
  },
  TIMMsgRevoke: function TIMMsgRevoke(conv_id, conv_type, message_param, cb, user_data) {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    var prefix = converter.fromConvType(conv_type);
    var convID = UTF8ToString(conv_id);
    if (!convID.startsWith(prefix)) {
      convID = prefix + convID;
    }
    var msg = JSON.parse(UTF8ToString(message_param));
    var msgJS = tim.findMessage(msg.message_msg_id);
    tim.revokeMessage(msgJS).then(function (imResponse) {
      Module['dynCall_viiii'](cb, imResponse.code, '', '', user_data);
    }).catch(function (imError) {
      console.warn('revokeMessage error:', imError);
    });
  },
  TIMMsgModifyMessage: function TIMMsgModifyMessage(message_param, cb, user_data) {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    var msg = JSON.parse(UTF8ToString(message_param));
    var modifiedPayload = converter.fromElement(msg.message_elem_array[0]);
    var msgJS = tim.findMessage(msg.message_msg_id);
    msgJS.payload = modifiedPayload;
    tim.modifyMessage(msgJS).then(function (imResponse) {
      var message = imResponse.data.message;
      var msgStr = JSON.stringify(converter.toMessage(message));
      var msg = utils.getStrBuf(msgStr);
      Module['dynCall_viiii'](cb, imResponse.code, '', msg, user_data);
    }).catch(function (imError) {
      console.warn('revokeMessage error:', imError);
    });
  },
  TIMMsgFindByMsgLocatorList: function TIMMsgFindByMsgLocatorList() {
    utils._handleNotImplemented();
  },
  TIMMsgImportMsgList: function TIMMsgImportMsgList() {
    utils._handleNotImplemented();
  },
  TIMMsgSaveMsg: function TIMMsgSaveMsg() {
    utils._handleNotImplemented();
  },
  TIMMsgGetMsgList: function TIMMsgGetMsgList(conv_id, conv_type, message_param, cb, user_data) {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    var prefix = converter.fromConvType(conv_type);
    var convID = UTF8ToString(conv_id);
    if (!convID.startsWith(prefix)) {
      convID = prefix + convID;
    }
    var param = JSON.parse(UTF8ToString(message_param));
    if (!param.msg_getmsglist_param_time_begin && !param.msg_getmsglist_param_last_msg_seq) {
      var opt = {
        conversationID: convID
      };
      if (param.msg_getmsglist_param_last_msg) {
        opt.nextReqMessageID = param.msg_getmsglist_param_last_msg.message_msg_id;
      }
      tim.getMessageList(opt).then(function (imResponse) {
        var messageList = imResponse.data.messageList;
        if (!messageList) {
          messageList = [];
        }
        // toMessage
        var formattedMessageList = messageList.map(function (msg) {
          return converter.toMessage(msg);
        });
        var buf = utils.getStrBuf(formattedMessageList);
        Module['dynCall_viiii'](cb, imResponse.code, '', buf, user_data);
      });
    } else {
      var option = {
        conversationID: convID
      };
      if (param.msg_getmsglist_param_is_forward) {
        option.direction = 1;
      }
      if (param.msg_getmsglist_param_last_msg_seq) {
        option.sequence = param.msg_getmsglist_param_last_msg_seq;
      }
      if (param.msg_getmsglist_param_time_begin) {
        option.time = param.msg_getmsglist_param_time_begin;
      }
      option.count = param.msg_getmsglist_param_count ? param.msg_getmsglist_param_count : 15;
      var promise = tim.getMessageListHopping(option);
      promise.then(function (imResponse) {
        var _imResponse$data = imResponse.data,
          _imResponse$data$mess = _imResponse$data.messageList,
          messageList = _imResponse$data$mess === void 0 ? [] : _imResponse$data$mess,
          isCompleted = _imResponse$data.isCompleted,
          nextMessageSeq = _imResponse$data.nextMessageSeq;
        var formattedMessageList = messageList.map(function (msg) {
          return converter.toMessage(msg);
        });
        var buf = utils.getStrBuf(formattedMessageList);
        Module['dynCall_viiii'](cb, imResponse.code, '', buf, user_data);
      });
    }
  },

  TIMMsgDelete: function TIMMsgDelete(conv_id, conv_type, json_msgdel_param, cb, user_data) {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    var messageID = JSON.parse(UTF8ToString(json_msgdel_param)).msg_delete_param_msg.message_msg_id;
    var messageList = [tim.findMessage(messageID)];
    var promise = tim.deleteMessage(messageList);
    promise.then(function (imResponse) {
      Module['dynCall_viiii'](cb, imResponse.code, '', '', user_data);
    }).catch(function (imError) {
      console.warn('deleteMessage error:', imError);
    });
  },
  TIMMsgListDelete: function TIMMsgListDelete(conv_id, conv_type, message_list, cb, user_data) {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    var messages = JSON.parse(UTF8ToString(message_list));
    var messageList = messages.map(function (msg) {
      tim.findMessage(msg.message_msg_id);
    });
    var promise = tim.deleteMessage(messageList);
    promise.then(function (imResponse) {
      Module['dynCall_viiii'](cb, imResponse.code, '', '', user_data);
    }).catch(function (imError) {
      console.warn('deleteMessage error:', imError);
    });
  },
  TIMMsgClearHistoryMessage: function TIMMsgClearHistoryMessage() {
    utils._handleNotImplemented();
  },
  TIMMsgSetC2CReceiveMessageOpt: function TIMMsgSetC2CReceiveMessageOpt(json_identifier_array, opt, cb, user_data) {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    var userIDList = JSON.parse(UTF8ToString(json_identifier_array));
    var promise = tim.setMessageRemindType({
      userIDList: userIDList,
      messageRemindType: converter.fromReceiveMsgOpt(opt)
    });
    promise.then(function (imResponse) {
      Module['dynCall_viiii'](cb, imResponse.code, '', '', user_data);
    }).catch(function (imError) {
      console.warn('setMessageRemindType error:', imError);
    });
  },
  TIMMsgGetC2CReceiveMessageOpt: function TIMMsgGetC2CReceiveMessageOpt() {
    utils._handleNotImplemented();
  },
  TIMMsgSetGroupReceiveMessageOpt: function TIMMsgSetGroupReceiveMessageOpt(group_id, opt, cb, user_data) {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    var groupID = UTF8ToString(group_id);
    var promise = tim.setMessageRemindType({
      groupID: groupID,
      messageRemindType: converter.fromReceiveMsgOpt(opt)
    });
    promise.then(function (imResponse) {
      Module['dynCall_viiii'](cb, imResponse.code, '', '', user_data);
    }).catch(function (imError) {
      console.warn('setMessageRemindType error:', imError);
    });
  },
  TIMMsgDownloadElemToPath: function TIMMsgDownloadElemToPath() {
    utils._handleNotImplemented();
  },
  TIMMsgDownloadMergerMessage: function TIMMsgDownloadMergerMessage(json_single_msg, cb, user_data) {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    var message = JSON.parse(UTF8ToString(json_single_msg));
    var messageID = message.message_msg_id;
    var foundMessage = tim.findMessage(messageID);
    if (foundMessage.type === TIM.TYPES.MSG_MERGER && foundMessage.payload.downloadKey !== '') {
      var promise = tim.downloadMergerMessage(foundMessage);
      promise.then(function (imResponse) {
        console.log(imResponse.data);
        var msgList = foundMessage.payload.messageList.map(function (msg) {
          return converter.toMessage(msg);
        });
        var buf = utils.getStrBuf(msgList);
        Module['dynCall_viiii'](cb, imResponse.code, '', buf, user_data);
      }).catch(function (imError) {
        console.warn('downloadMergerMessage error:', imError);
      });
    }
  },
  TIMMsgBatchSend: function TIMMsgBatchSend() {
    utils._handleNotImplemented();
  },
  TIMMsgSearchLocalMessages: function TIMMsgSearchLocalMessages() {
    utils._handleNotImplemented();
  },
  TIMMsgSetLocalCustomData: function TIMMsgSetLocalCustomData() {
    utils._handleNotImplemented();
  },
  TIMGroupCreate: function TIMGroupCreate(json_group_create_param, cb, user_data) {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    var param = JSON.parse(UTF8ToString(json_group_create_param));
    var formattedParam = converter.fromCreateGroup(param);
    var promise = tim.createGroup(formattedParam);
    promise.then(function (imResponse) {
      console.log(imResponse.data.group);
      console.log(imResponse.data.overLimitUserIDList);
      Module['dynCall_viiii'](cb, imResponse.code, '', '', user_data);
    }).catch(function (imError) {
      console.warn('createGroup error:', imError);
    });
  },

  TIMGroupDelete: function TIMGroupDelete(group_id, cb, user_data) {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    var groupID = UTF8ToString(group_id);
    var promise = tim.dismissGroup(groupID);
    promise.then(function (imResponse) {
      console.log(imResponse.data.groupID);
      Module['dynCall_viiii'](cb, imResponse.code, '', '', user_data);
    }).catch(function (imError) {
      console.warn('dismissGroup error:', imError); 
    });
  },

  TIMGroupJoin: function TIMGroupJoin(group_id, hello_message, cb, user_data) {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    var promise = tim.joinGroup({
      groupID: UTF8ToString(group_id),
      applyMessage: UTF8ToString(hello_message)
    });
    promise.then(function (imResponse) {
      switch (imResponse.data.status) {
        case TIM.TYPES.JOIN_STATUS_WAIT_APPROVAL:
          break;
        case TIM.TYPES.JOIN_STATUS_SUCCESS:
          console.log(imResponse.data.group); 
          break;
        case TIM.TYPES.JOIN_STATUS_ALREADY_IN_GROUP:
          break;
        default:
          break;
      }
      Module['dynCall_viiii'](cb, imResponse.code, '', '', user_data);
    }).catch(function (imError) {
      console.warn('joinGroup error:', imError); 
    });
  },

  TIMGroupQuit: function TIMGroupQuit(group_id, cb, user_data) {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    var promise = tim.quitGroup(UTF8ToString(group_id));
    promise.then(function (imResponse) {
      console.log(imResponse.data.groupID);
      Module['dynCall_viiii'](cb, imResponse.code, '', '', user_data);
    }).catch(function (imError) {
      console.warn('quitGroup error:', imError); 
    });
  },

  TIMGroupInviteMember: function TIMGroupInviteMember(json_group_invite_param, cb, user_data) {
    // List<GroupInviteMemberResult>
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    var param = JSON.parse(UTF8ToString(json_group_invite_param));
    var promise = tim.addGroupMember({
      groupID: param.group_invite_member_param_group_id,
      userIDList: param.group_invite_member_param_identifier_array
    });
    promise.then(function (imResponse) {
      console.log(imResponse.data.successUserIDList);
      console.log(imResponse.data.failureUserIDList); 
      console.log(imResponse.data.existedUserIDList);
      console.log(imResponse.data.overLimitUserIDList);
      console.log(imResponse.data.group);
      var res = [];
      imResponse.data.failureUserIDList.forEach(function (userID) {
        res.push({
          group_invite_member_result_identifier: userID,
          group_invite_member_result_result: 0
        });
      });
      imResponse.data.successUserIDList.forEach(function (userID) {
        res.push({
          group_invite_member_result_identifier: userID,
          group_invite_member_result_result: 1
        });
      });
      imResponse.data.existedUserIDList.forEach(function (userID) {
        res.push({
          group_invite_member_result_identifier: userID,
          group_invite_member_result_result: 2
        });
      });
      var buf = utils.getStrBuf(res);
      Module['dynCall_viiii'](cb, imResponse.code, '', buf, user_data);
    }).catch(function (imError) {
      console.warn('addGroupMember error:', imError);
    });
  },

  TIMGroupDeleteMember: function TIMGroupDeleteMember(json_group_delete_param, cb, user_data) {
    // List<GroupDeleteMemberResult>
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    var param = JSON.parse(UTF8ToString(json_group_delete_param));
    var promise = tim.deleteGroupMember({
      groupID: String(param.group_delete_member_param_group_id),
      userIDList: param.group_delete_member_param_identifier_array,
      reason: param.group_delete_member_param_user_data
    });
    promise.then(function (imResponse) {
      console.log(imResponse.data.group);
      console.log(imResponse.data.userIDList); 
      var res = [];
      imResponse.data.userIDList.forEach(function (userID) {
        res.push({
          group_delete_member_result_identifier: userID,
          group_delete_member_result_result: 1
        });
      });
      var buf = utils.getStrBuf(res);
      Module['dynCall_viiii'](cb, imResponse.code, '', buf, user_data);
    }).catch(function (imError) {
      console.warn('deleteGroupMember error:', imError);
    });
  },

  TIMGroupGetJoinedGroupList: function TIMGroupGetJoinedGroupList(cb, user_data) {
    // List<GroupBaseInfo>
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    var promise = tim.getGroupList();
    promise.then(function (imResponse) {
      console.log(imResponse.data.groupList); 
      var groupList = imResponse.data.groupList.map(function (group) {
        return converter.toGroupBaseInfo(group);
      });
      var buf = utils.getStrBuf(groupList);
      Module['dynCall_viiii'](cb, imResponse.code, '', buf, user_data);
    }).catch(function (imError) {
      console.warn('getGroupList error:', imError); 
    });
  },

  TIMGroupGetGroupInfoList: function TIMGroupGetGroupInfoList(group_id_list, cb, user_data) {
    // List<GetGroupInfoResult>
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    var ids = JSON.parse(UTF8ToString(group_id_list));
    var promises = ids.map(function (id) {
      return tim.getGroupProfile({
        groupID: id
      });
    });
    Promise.all(promises).then(function (imResponses) {
      console.log(imResponses);
      var groupDetails = imResponses.map(function (imResponse) {
        return {
          get_groups_info_result_code: imResponse.code,
          get_groups_info_result_desc: '',
          get_groups_info_result_info: converter.toGroupDetailInfo(imResponse.data.group)
        };
      });
      var buf = utils.getStrBuf(groupDetails);
      Module['dynCall_viiii'](cb, 0, '', buf, user_data);
    }).catch(function (imError) {
      console.warn('getGroupProfile error:', imError); 
    });
  },

  TIMGroupModifyGroupInfo: function TIMGroupModifyGroupInfo(json_group_modifyinfo_param, cb, user_data) {
    // null
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    var param = JSON.parse(UTF8ToString(json_group_modifyinfo_param));
    if (param.group_modify_info_param_owner) {
      var promise = tim.changeGroupOwner({
        groupID: param.group_modify_info_param_group_id,
        newOwnerID: param.group_modify_info_param_owner
      });
      promise.then(function (imResponse) {
        console.log(imResponse.data.group); 
        Module['dynCall_viiii'](cb, imResponse.code, '', '', user_data);
      }).catch(function (imError) {
        console.warn('changeGroupOwner error:', imError);
      });
    } else {
      var option = converter.fromGroupModifyInfoParam(param);
      var _promise5 = tim.updateGroupProfile(option);
      _promise5.then(function (imResponse) {
        console.log(imResponse.data.group); 
        Module['dynCall_viiii'](cb, imResponse.code, '', '', user_data);
      }).catch(function (imError) {
        console.warn('updateGroupProfile error:', imError);
      });
    }
  },

  TIMGroupGetMemberInfoList: function TIMGroupGetMemberInfoList(json_group_getmeminfos_param, cb, user_data) {
    // GroupGetMemberInfoListResult
    // getGroupMemberList
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    var param = JSON.parse(UTF8ToString(json_group_getmeminfos_param));
    var options = {};
    options.groupID = param.group_get_members_info_list_param_group_id;
    if (param.group_get_members_info_list_param_identifier_array) {
      // getGroupMemberProfile
      options.userIDList = param.group_get_members_info_list_param_identifier_array;
      if (param.group_get_members_info_list_param_option && param.group_get_members_info_list_param_option.group_member_get_info_option_custom_array) {
        options.memberCustomFieldFilter = param.group_get_members_info_list_param_option.group_member_get_info_option_custom_array;
      }
      var promise = tim.getGroupMemberProfile(options);
      promise.then(function (imResponse) {
        console.log(imResponse.data.memberList); 
        var res = {
          group_get_memeber_info_list_result_next_seq: 0,
          group_get_memeber_info_list_result_info_array: imResponse.data.memberList.map(function (member) {
            return converter.toGroupMemberInfo(member, options.groupID);
          })
        };
        var buf = utils.getStrBuf(res);
        Module['dynCall_viiii'](cb, imResponse.code, '', buf, user_data);
      }).catch(function (imError) {
        console.warn('getGroupMemberProfile error:', imError);
      });
    } else {
      // getGroupMemberList
      var offset = param.group_get_memeber_info_list_result_next_seq ? param.group_get_memeber_info_list_result_next_seq : 0;
      options.offset = offset;
      var count = 15;
      options.count = count;
      var _promise6 = tim.getGroupMemberList(options); 
      _promise6.then(function (imResponse) {
        console.log(imResponse.data.memberList);
        var res = {
          group_get_memeber_info_list_result_next_seq: offset + count,
          group_get_memeber_info_list_result_info_array: imResponse.data.memberList.map(function (member) {
            return converter.toGroupMemberInfo(member, options.groupID);
          })
        };
        var buf = utils.getStrBuf(res);
        Module['dynCall_viiii'](cb, imResponse.code, '', buf, user_data);
      }).catch(function (imError) {
        console.warn('getGroupMemberList error:', imError);
      });
    }
  },
  TIMGroupMarkGroupMemberList: function TIMGroupMarkGroupMemberList() {
    utils._handleNotImplemented();
  },
  TIMGroupModifyMemberInfo: function TIMGroupModifyMemberInfo(json_group_modifymeminfo_param, cb, user_data) {
    // null
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    var param = JSON.parse(UTF8ToString(json_group_modifymeminfo_param));
    var promises = [];
    if (param.group_modify_member_info_member_role !== undefined) {
      promises.push(tim.setGroupMemberRole({
        groupID: param.group_modify_member_info_group_id,
        userID: param.group_modify_member_info_identifier,
        role: converter.fromRole(param.group_modify_member_info_member_role)
      }));
    }
    if (param.group_modify_member_info_shutup_time !== undefined) {
      promises.push(tim.setGroupMemberMuteTime({
        groupID: param.group_modify_member_info_group_id,
        userID: param.group_modify_member_info_identifier,
        muteTime: param.group_modify_member_info_shutup_time
      }));
    }
    if (param.group_modify_member_info_name_card) {
      promises.push(tim.setGroupMemberNameCard({
        groupID: param.group_modify_member_info_group_id,
        userID: param.group_modify_member_info_identifier,
        nameCard: param.group_modify_member_info_name_card
      }));
    }
    if (param.group_modify_member_info_custom_info) {
      promises.push(tim.setGroupMemberCustomField({
        groupID: param.group_modify_member_info_group_id,
        memberCustomField: param.group_modify_member_info_custom_info.map(function (custom) {
          return {
            key: custom.group_member_info_custom_string_info_key,
            value: custom.group_member_info_custom_string_info_value
          };
        })
      }));
    }
    Promise.all(promises).then(function (imResponses) {
      console.log(imResponses);
      Module['dynCall_viiii'](cb, 0, '', '', user_data);
    }).catch(function (imError) {
      console.warn('setGroupMember error:', imError);
    });
  },
  TIMGroupReportPendencyReaded: function TIMGroupReportPendencyReaded() {
    utils._handleNotImplemented();
  },
  TIMGroupGetPendencyList: function TIMGroupGetPendencyList(json_group_getpendence_list_param, cb, user_data) {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    if (!tim.getGroupApplicationList) {
      console.warn('Please update web sdk up to v.2.24.0.');
      return;
    }
    // GroupPendencyResult
    var promise = tim.getGroupApplicationList();
    promise.then(function (imResponse) {
      var applicationList = imResponse.data.applicationList ? imResponse.data.applicationList : [];
      var res = {
        group_pendency_result_pendency_array: applicationList.map(function (item) {
          return {
            group_pendency_group_id: item.groupID,
            group_pendency_form_identifier: item.applicant,
            group_pendency_authentication: item.authentication,
            group_pendency_key: String(item.messageKey)
          };
        })
      };
      var buf = utils.getStrBuf(res);
      Module['dynCall_viiii'](cb, imResponse.code, '', buf, user_data);
    }).catch(function (imError) {
      console.warn('getGroupApplicationList error:', imError);
    });
  },
  TIMGroupHandlePendency: function TIMGroupHandlePendency(json_group_handle_pendency_param, cb, user_data) {
    // null
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    var param = JSON.parse(UTF8ToString(json_group_handle_pendency_param));
    var groupHandleParam = param.group_handle_pendency_param_pendency;
    var application = {
      applicant: groupHandleParam.group_pendency_form_identifier,
      applicantNick: '',
      groupName: '',
      groupID: groupHandleParam.group_pendency_group_id,
      authentication: groupHandleParam.group_pendency_authentication,
      messageKey: Number(groupHandleParam.group_pendency_key)
    };
    var promise = tim.handleGroupApplication({
      handleAction: param.group_handle_pendency_param_is_accept ? 'Agree' : 'Reject',
      handleMessage: param.group_handle_pendency_param_handle_msg,
      application: application
    });
    promise.then(function (imResponse) {
      console.log(imResponse.data.group);
      Module['dynCall_viiii'](cb, imResponse.code, '', '', user_data);
    }).catch(function (imError) {
      console.warn('handleGroupApplication error:', imError); 
    });
  },

  TIMGroupGetOnlineMemberCount: function TIMGroupGetOnlineMemberCount(group_id, cb, user_data) {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    var promise = tim.getGroupOnlineMemberCount(UTF8ToString(group_id));
    promise.then(function (imResponse) {
      console.log(imResponse.data.memberCount);
      var res = {
        group_get_online_member_count_result: imResponse.data.memberCount
      };
      var buf = utils.getStrBuf(res);
      Module['dynCall_viiii'](cb, imResponse.code, '', buf, user_data);
    }).catch(function (imError) {
      console.warn('getGroupOnlineMemberCount error:', imError); 
    });
  },

  TIMGroupSearchGroups: function TIMGroupSearchGroups(json_group_search_groups_param, cb, user_data) {
    // List<GroupDetailInfo>
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    var param = JSON.parse(UTF8ToString(json_group_search_groups_param));
    var promises = [];
    var len = Math.min(param.group_search_params_keyword_list.length, 5);
    for (var i = 0; i < len; i++) {
      promises.push(tim.searchGroupByID(param.group_search_params_keyword_list[i]));
    }
    Promise.all(promises).then(function (imResponses) {
      var res = imResponses.map(function (imResponse) {
        return converter.toGroupDetailInfo(imResponse.data.group);
      });
      var buf = utils.getStrBuf(res);
      Module['dynCall_viiii'](cb, 0, '', buf, user_data);
    }).catch(function (imError) {
      console.warn('searchGroupByID error:', imError); 
    });
  },

  TIMGroupSearchGroupMembers: function TIMGroupSearchGroupMembers() {
    utils._handleNotImplemented();
  },
  TIMGroupInitGroupAttributes: function TIMGroupInitGroupAttributes(group_id, json_group_atrributes, cb, user_data) {
    // null
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    var groupID = UTF8ToString(group_id);
    var attrs = JSON.parse(UTF8ToString(json_group_atrributes));
    var opt = {
      groupID: groupID,
      groupAttributes: {}
    };
    attrs.forEach(function (attr) {
      opt.groupAttributes[attr.group_atrribute_key] = attr.group_atrribute_value;
    });
    var promise = tim.initGroupAttributes(opt);
    promise.then(function (imResponse) {
      console.log(imResponse.data.groupAttributes); 
      Module['dynCall_viiii'](cb, imResponse.code, '', '', user_data);
    }).catch(function (imError) {
      console.warn('initGroupAttributes error:', imError);
    });
  },

  TIMGroupSetGroupAttributes: function TIMGroupSetGroupAttributes(group_id, json_group_atrributes, cb, user_data) {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    var groupID = UTF8ToString(group_id);
    var attrs = JSON.parse(UTF8ToString(json_group_atrributes));
    var promise = tim.setGroupAttributes({
      groupID: groupID,
      groupAttributes: attrs.map(function (attr) {
        return _defineProperty({}, attr.group_atrribute_key, attr.group_atrribute_value);
      })
    });
    promise.then(function (imResponse) {
      console.log(imResponse.data.groupAttributes); 
      Module['dynCall_viiii'](cb, imResponse.code, '', '', user_data);
    }).catch(function (imError) {
      console.warn('setGroupAttributes error:', imError); 
    });
  },

  TIMGroupDeleteGroupAttributes: function TIMGroupDeleteGroupAttributes(group_id, json_keys, cb, user_data) {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    var groupID = UTF8ToString(group_id);
    var keys = JSON.parse(UTF8ToString(json_keys));
    var promise = tim.deleteGroupAttributes({
      groupID: groupID,
      keyList: keys
    });
    promise.then(function (imResponse) {
      console.log(imResponse.data.keyList); 
      Module['dynCall_viiii'](cb, imResponse.code, '', '', user_data);
    }).catch(function (imError) {
      console.warn('deleteGroupAttributes error:', imError); 
    });
  },

  TIMGroupGetGroupAttributes: function TIMGroupGetGroupAttributes(group_id, json_keys, cb, user_data) {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    // List<GroupAttributes>
    var groupID = UTF8ToString(group_id);
    var keys = JSON.parse(UTF8ToString(json_keys));
    var promise = tim.getGroupAttributes({
      groupID: groupID,
      keyList: keys
    });
    promise.then(function (imResponse) {
      console.log(imResponse.data.groupAttributes); 
      var res = Object.keys(imResponse.data.groupAttributes).map(function (key) {
        return {
          group_atrribute_key: key,
          group_atrribute_value: imResponse.data.groupAttributes[key]
        };
      });
      var buf = utils.getStrBuf(res);
      Module['dynCall_viiii'](cb, imResponse.code, '', buf, user_data);
    }).catch(function (imError) {
      console.warn('getGroupAttributes error:', imError);
    });
  },

  TIMGroupGetJoinedCommunityList: function TIMGroupGetJoinedCommunityList(cb, user_data) {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    // List<GroupInfo>
    var promise = tim.getJoinedCommunityList();
    promise.then(function (imResponse) {
      console.log(imResponse.data.groupList); 
      var groupList = imResponse.data.groupList.map(function (group) {
        var base = converter.toGroupBaseInfo(group);
        var res = converter.toGroupDetailInfo(group);
        Object.keys(base).forEach(function (key) {
          res[key] = base[key];
        });
        return res;
      });
      var buf = utils.getStrBuf(groupList);
      Module['dynCall_viiii'](cb, imResponse.code, '', buf, user_data);
    }).catch(function (imError) {
      console.warn('getJoinedCommunityList error:', imError); 
    });
  },

  TIMGroupCreateTopicInCommunity: function TIMGroupCreateTopicInCommunity(group_id, json_topic_info, cb, user_data) {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    // topic_id
    var groupID = UTF8ToString(group_id);
    var param = JSON.parse(UTF8ToString(json_topic_info));
    var promise = tim.createTopicInCommunity({
      groupID: groupID,
      topicName: param.group_topic_info_topic_name,
      topicID: param.group_topic_info_topic_id,
      avatar: param.group_topic_info_topic_face_url,
      notification: param.group_topic_info_notification,
      introduction: param.group_topic_info_introduction,
      customData: param.group_topic_info_custom_string
    });
    promise.then(function (imResponse) {
      console.log(imResponse.data.topicID); 
      var buf = utils.getStrBuf(imResponse.data.topicID);
      Module['dynCall_viiii'](cb, imResponse.code, '', buf, user_data);
    }).catch(function (imError) {
      console.warn('createTopicInCommunity error:', imError); 
    });
  },

  TIMGroupDeleteTopicFromCommunity: function TIMGroupDeleteTopicFromCommunity(group_id, json_topic_id_array, cb, user_data) {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    // List<GroupTopicOperationResult>
    var groupID = UTF8ToString(group_id);
    var ids = JSON.parse(UTF8ToString(json_topic_id_array));
    var promise = tim.deleteTopicFromCommunity({
      groupID: groupID,
      topicIDList: ids
    });
    promise.then(function (imResponse) {
      var _imResponse$data2 = imResponse.data,
        successTopicList = _imResponse$data2.successTopicList,
        failureTopicList = _imResponse$data2.failureTopicList;
      var res = [];
      successTopicList.forEach(function (item) {
        res.push({
          group_topic_operation_result_error_code: 0,
          group_topic_operation_result_topic_id: item.topicID
        });
      });
      failureTopicList.forEach(function (item) {
        res.push({
          group_topic_operation_result_error_code: item.code,
          group_topic_operation_result_topic_id: item.topicID,
          group_topic_operation_result_error_message: item.message
        });
      });
      var buf = utils.getStrBuf(res);
      Module['dynCall_viiii'](cb, imResponse.code, '', buf, user_data);
    }).catch(function (imError) {
      console.warn('deleteTopicFromCommunity error:', imError); 
    });
  },

  TIMGroupSetTopicInfo: function TIMGroupSetTopicInfo(json_topic_info, cb, user_data) {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    var param = JSON.parse(UTF8ToString(json_topic_info));

    tim.getJoinedCommunityList().then(function (imResponse) {
      var _imResponse$data$grou = imResponse.data.groupList,
        groupList = _imResponse$data$grou === void 0 ? [] : _imResponse$data$grou;
      if (!imResponse.data.groupList) {
        return;
      }
      var promises = groupList.map(function (group) {
        return tim.getTopicList({
          groupID: group.groupID
        });
      });
      Promise.all(promises).then(function (imResponses) {
        var promiseIdx = imResponses.findIndex(function (imResponse) {
          var idx = imResponse.data.successTopicList.findIndex(function (topic) {
            return param.group_topic_info_topic_id === topic.topicID;
          });
          return idx >= 0;
        });
        if (promiseIdx >= 0) {
          var groupID = groupList[promiseIdx];
          var opt = {
            groupID: groupID,
            topicID: param.group_topic_info_topic_id
          };
          if (param.group_topic_info_topic_name) {
            opt.topicName = param.group_topic_info_topic_name;
          }
          if (param.group_topic_info_notification) {
            opt.notification = param.group_topic_info_notification;
          }
          if (param.group_topic_info_introduction) {
            opt.introduction = param.group_topic_info_introduction;
          }
          if (param.group_topic_info_custom_string) {
            opt.customData = param.group_topic_info_custom_string;
          }
          if (param.group_topic_info_topic_face_url) {
            opt.avatar = param.group_topic_info_topic_face_url;
          }
          if (param.group_topic_info_is_all_muted !== undefined) {
            opt.muteAllMembers = param.group_topic_info_is_all_muted;
          }
          var promise = tim.updateTopicProfile(opt);
          promise.then(function (imResponse) {
            console.log(imResponse.data.topic); 
            Module['dynCall_viiii'](cb, imResponse.code, '', '', user_data);
          }).catch(function (imError) {
            console.warn('updateTopicProfile error:', imError); 
          });
        }
      }).catch(function (imError) {
        console.warn('getTopicListSync error:', imError);
      });
    });
  },
  TIMGroupGetTopicInfoList: function TIMGroupGetTopicInfoList(group_id, json_topic_id_array, cb, user_data) {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    // List<GroupTopicInfo>
    var groupID = UTF8ToString(group_id);
    var ids = JSON.parse(UTF8ToString(json_topic_id_array));
    var promise = tim.getTopicList({
      groupID: groupID,
      topicIDList: ids
    });
    promise.then(function (imResponse) {
      var _imResponse$data3 = imResponse.data,
        successTopicList = _imResponse$data3.successTopicList,
        failureTopicList = _imResponse$data3.failureTopicList;
      var res = successTopicList.map(function (topic) {
        return converter.toGroupTopicInfo(topic);
      });
      var buf = utils.getStrBuf(res);
      Module['dynCall_viiii'](cb, imResponse.code, '', buf, user_data);
      // failureTopicList.forEach((item) => {
      //   const { topicID, code, message } = item;
      // })
    }).catch(function (imError) {
      console.warn('getTopicList error:', imError); 
    });
  },

  TIMGetUserStatus: function TIMGetUserStatus(json_identifier_array, cb, user_data) {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    // List<UserStatus>
    var ids = JSON.parse(UTF8ToString(json_identifier_array));
    var promise = tim.getUserStatus({
      userIDList: ids
    });
    promise.then(function (imResponse) {
      var _imResponse$data4 = imResponse.data,
        successUserList = _imResponse$data4.successUserList,
        failureUserList = _imResponse$data4.failureUserList;
      var res = successUserList.map(function (item) {
        return {
          user_status_identifier: item.userID,
          user_status_status_type: converter.toUserStatusType(item.statusType),
          user_status_custom_status: item.customStatus
        };
      });
      var buf = utils.getStrBuf(res);
      Module['dynCall_viiii'](cb, imResponse.code, '', buf, user_data);
      // failureUserList.forEach((item) => {
      //   const { userID, code, message } = item;
      // });
    }).catch(function (imError) {
      console.warn('getUserStatus error:', imError); 
    });
  },

  TIMSetSelfStatus: function TIMSetSelfStatus(json_current_user_status, cb, user_data) {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    var param = JSON.parse(UTF8ToString(json_current_user_status));
    var promise = tim.setSelfStatus({
      customStatus: param.user_status_custom_status
    });
    promise.then(function (imResponse) {
      console.log(imResponse.data);
      // const { userID, statusType, customStatus } = imResponse.data;
      Module['dynCall_viiii'](cb, imResponse.code, '', '', user_data);
    }).catch(function (imError) {
      console.warn('setSelfStatus error:', imError); 
    });
  },

  TIMSubscribeUserStatus: function TIMSubscribeUserStatus(json_identifier_array, cb, user_data) {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    var ids = JSON.parse(UTF8ToString(json_identifier_array));
    var promise = tim.subscribeUserStatus({
      userIDList: ids
    });
    promise.then(function (imResponse) {
      var failureUserList = imResponse.data.failureUserList;
      failureUserList.forEach(function (item) {
        // const { userID, code, message } = item;
      });
      Module['dynCall_viiii'](cb, imResponse.code, '', '', user_data);
    }).catch(function (imError) {
      console.warn('subscribeUserStatus error:', imError); 
    });
  },

  TIMUnsubscribeUserStatus: function TIMUnsubscribeUserStatus(json_identifier_array, cb, user_data) {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    var ids = JSON.parse(UTF8ToString(json_identifier_array));
    var promise = tim.unsubscribeUserStatus({
      userIDList: ids
    });
    promise.then(function (imResponse) {
      var failureUserList = imResponse.data.failureUserList;
      failureUserList.forEach(function (item) {
        // const { userID, code, message } = item;
      });
      Module['dynCall_viiii'](cb, imResponse.code, '', '', user_data);
    }).catch(function (imError) {
      console.warn('subscribeUserStatus error:', imError);
    });
  },

  TIMProfileGetUserProfileList: function TIMProfileGetUserProfileList(json_get_user_profile_list_param, cb, user_data) {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    // List<UserProfile>
    var param = JSON.parse(UTF8ToString(json_get_user_profile_list_param));
    var promise = tim.getUserProfile({
      userIDList: param.friendship_getprofilelist_param_identifier_array 
    });

    promise.then(function (imResponse) {
      console.log(imResponse.data); 
      var res = imResponse.data.map(function (profile) {
        return converter.toUserProfile(profile);
      });
      var buf = utils.getStrBuf(res);
      Module['dynCall_viiii'](cb, imResponse.code, '', buf, user_data);
    }).catch(function (imError) {
      console.warn('getUserProfile error:', imError); 
    });
  },

  TIMProfileModifySelfUserProfile: function TIMProfileModifySelfUserProfile(json_modify_self_user_profile_param, cb, user_data) {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    var param = JSON.parse(UTF8ToString(json_modify_self_user_profile_param));
    var opt = {};
    if (param.user_profile_item_nick_name) {
      opt.nick = param.user_profile_item_nick_name;
    }
    if (param.user_profile_item_gender !== undefined) {
      opt.gender = converter.fromGender(param.user_profile_item_gender);
    }
    if (param.user_profile_item_face_url) {
      opt.avatar = param.user_profile_item_face_url;
    }
    if (param.user_profile_item_self_signature) {
      opt.selfSignature = param.user_profile_item_self_signature;
    }
    if (param.user_profile_item_add_permission !== undefined) {
      opt.allowType = converter.fromAddPermission(param.user_profile_item_add_permission);
    }
    if (param.user_profile_item_location) {
      opt.location = param.user_profile_item_location;
    }
    if (param.user_profile_item_language !== undefined) {
      opt.language = param.user_profile_item_language;
    }
    if (param.user_profile_item_birthday) {
      opt.birthday = param.user_profile_item_birthday;
    }
    if (param.user_profile_item_level !== undefined) {
      opt.level = param.user_profile_item_level;
    }
    if (param.user_profile_item_role !== undefined) {
      opt.role = param.user_profile_item_role;
    }
    if (param.user_profile_item_custom_string_array) {
      opt.profileCustomField = param.user_profile_item_custom_string_array.map(function (cf) {
        return {
          key: cf.user_profile_custom_string_info_key,
          value: cf.user_profile_custom_string_info_value
        };
      });
    }
    var promise = tim.updateMyProfile(opt);
    promise.then(function (imResponse) {
      console.log(imResponse.data);
      Module['dynCall_viiii'](cb, imResponse.code, '', '', user_data);
    }).catch(function (imError) {
      console.warn('updateMyProfile error:', imError); 
    });
  },

  TIMFriendshipGetFriendProfileList: function TIMFriendshipGetFriendProfileList(cb, user_data) {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    // List<FriendProfile>
    var promise = tim.getFriendList();
    promise.then(function (imResponse) {
      var friendList = imResponse.data; 
      store.friendList = friendList;
      if (cb) {
        var res = friendList.map(function (friend) {
          return converter.toFriendProfile(friend);
        });
        var buf = utils.getStrBuf(res);
        Module['dynCall_viiii'](cb, imResponse.code, '', buf, user_data);
      }
    }).catch(function (imError) {
      console.warn('getFriendList error:', imError); 
    });
  },

  TIMFriendshipAddFriend: function TIMFriendshipAddFriend(json_add_friend_param, cb, user_data) {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    // FriendResult
    var param = JSON.parse(UTF8ToString(json_add_friend_param));
    var source = param.friendship_add_friend_param_add_source;
    if (!source) {
      source = 'AddSource_Type_WebGL';
    }
    var opt = {
      to: param.friendship_add_friend_param_identifier,
      source: source,
      remark: param.friendship_add_friend_param_remark,
      groupName: param.friendship_add_friend_param_group_name,
      wording: param.friendship_add_friend_param_add_wording,
      type: converter.fromFriendType(param.friendship_add_friend_param_friend_type)
    };
    var promise = tim.addFriend(opt);
    promise.then(function (imResponse) {
      var _imResponse$data5 = imResponse.data,
        code = _imResponse$data5.code,
        userID = _imResponse$data5.userID,
        message = _imResponse$data5.message;
      if (code === 30539) {
      } else if (code === 0) {
      }
      var res = {
        friend_result_identifier: userID,
        friend_result_code: code,
        friend_result_desc: message
      };
      var buf = utils.getStrBuf(res);
      Module['dynCall_viiii'](cb, imResponse.code, '', buf, user_data);
    }).catch(function (imError) {
      console.warn('addFriend error:', imError); 
    });
  },

  TIMFriendshipHandleFriendAddRequest: function TIMFriendshipHandleFriendAddRequest(json_handle_friend_add_param, cb, user_data) {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    // FriendResult
    var param = JSON.parse(UTF8ToString(json_handle_friend_add_param));
    var opt = {
      userID: param.friend_respone_identifier
    };
    if (param.friend_respone_action === 2) {
      // Reject
      var promise = tim.refuseFriendApplication(opt);
      promise.then(function (imResponse) {
        var res = {
          friend_result_identifier: param.friend_respone_identifier,
          friend_result_code: 0,
          friend_result_desc: 'success'
        };
        var buf = utils.getStrBuf(res);
        Module['dynCall_viiii'](cb, 0, '', buf, user_data);
      }).catch(function (imError) {
        console.warn('refuseFriendApplication error:', imError);
      });
    } else {
      if (param.friend_respone_action === 0) {
        opt.type = TIM.TYPES.SNS_APPLICATION_AGREE;
      } else {
        opt.type = TIM.TYPES.SNS_APPLICATION_AGREE_AND_ADD;
      }
      if (param.friend_respone_remark) {
        opt.remark = param.friend_respone_remark;
      }
      var _promise7 = tim.acceptFriendApplication(opt);
      _promise7.then(function (imResponse) {
        var res = {
          friend_result_identifier: param.friend_respone_identifier,
          friend_result_code: 0,
          friend_result_desc: 'success'
        };
        var buf = utils.getStrBuf(res);
        Module['dynCall_viiii'](cb, 0, '', buf, user_data);
      }).catch(function (imError) {
        console.warn('acceptFriendApplication error:', imError);
      });
    }
  },
  TIMFriendshipModifyFriendProfile: function TIMFriendshipModifyFriendProfile(json_modify_friend_info_param, cb, user_data) {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    var param = JSON.parse(UTF8ToString(json_modify_friend_info_param));
    var opt = {
      userID: param.friendship_modify_friend_profile_param_identifier
    };
    var item = param.friendship_modify_friend_profile_param_item;
    if (item.friend_profile_item_remark) {
      opt.remark = item.friend_profile_item_remark;
    }
    if (item.friend_profile_item_custom_string_array) {
      opt.friendCustomField = item.friend_profile_item_custom_string_array.map(function (cf) {
        return {
          key: cf.friend_profile_custom_string_info_key,
          value: cf.friend_profile_custom_string_info_value
        };
      });
    }
    var promise = tim.updateFriend(opt);
    promise.then(function (imResponse) {
      console.log(imResponse.data); 
      Module['dynCall_viiii'](cb, imResponse.code, '', '', user_data);
    }).catch(function (imError) {
      console.warn('getFriendProfile error:', imError);
    });
  },

  TIMFriendshipDeleteFriend: function TIMFriendshipDeleteFriend(json_delete_friend_param, cb, user_data) {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    // List<FriendResult>
    var param = JSON.parse(UTF8ToString(json_delete_friend_param));
    var promise = tim.deleteFriend({
      userIDList: param.friendship_delete_friend_param_identifier_array,
      type: converter.fromFriendType(param.friendship_delete_friend_param_friend_type)
    });
    promise.then(function (imResponse) {
      var _imResponse$data6 = imResponse.data,
        successUserIDList = _imResponse$data6.successUserIDList,
        failureUserIDList = _imResponse$data6.failureUserIDList;
      var res = [];
      successUserIDList.forEach(function (item) {
        res.push({
          friend_result_identifier: item.userID,
          friend_result_code: 0,
          friend_result_desc: 'success'
        });
      });
      failureUserIDList.forEach(function (item) {
        res.push({
          friend_result_identifier: item.userID,
          friend_result_code: item.code,
          friend_result_desc: item.message
        });
      });
      var buf = utils.getStrBuf(res);
      Module['dynCall_viiii'](cb, imResponse.code, '', buf, user_data);
    }).catch(function (imError) {
      console.warn('removeFromFriendList error:', imError);
    });
  },
  TIMFriendshipCheckFriendType: function TIMFriendshipCheckFriendType(json_check_friend_list_param, cb, user_data) {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    // List<FriendshipCheckFriendTypeResult>
    var param = JSON.parse(UTF8ToString(json_check_friend_list_param));
    var promise = tim.checkFriend({
      userIDList: param.friendship_check_friendtype_param_identifier_array,
      type: converter.fromFriendType(param.friendship_check_friendtype_param_check_type)
    });
    promise.then(function (imResponse) {
      var _imResponse$data7 = imResponse.data,
        successUserIDList = _imResponse$data7.successUserIDList,
        failureUserIDList = _imResponse$data7.failureUserIDList;
      var res = [];
      successUserIDList.forEach(function (item) {
        res.push({
          friendship_check_friendtype_result_identifier: item.userID,
          friendship_check_friendtype_result_relation: converter.toCheckRelation(item.relation),
          friendship_check_friendtype_result_code: item.code,
          friendship_check_friendtype_result_desc: 'success'
        });
      });
      failureUserIDList.forEach(function (item) {
        res.push({
          friendship_check_friendtype_result_identifier: item.userID,
          friendship_check_friendtype_result_code: item.code,
          friendship_check_friendtype_result_desc: item.message
        });
      });
      var buf = utils.getStrBuf(res);
      Module['dynCall_viiii'](cb, imResponse.code, '', buf, user_data);
    }).catch(function (imError) {
      console.warn('checkFriend error:', imError);
    });
  },
  TIMFriendshipCreateFriendGroup: function TIMFriendshipCreateFriendGroup(json_create_friend_group_param, cb, user_data) {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    // List<FriendResult>
    var param = JSON.parse(UTF8ToString(json_create_friend_group_param));
    var promise = tim.createFriendGroup({
      name: param.friendship_create_friend_group_param_name_array[0],
      userIDList: param.friendship_create_friend_group_param_identifier_array
    });
    promise.then(function (imResponse) {
      var _imResponse$data8 = imResponse.data,
        friendGroup = _imResponse$data8.friendGroup,
        _imResponse$data8$fai = _imResponse$data8.failureUserIDList,
        failureUserIDList = _imResponse$data8$fai === void 0 ? [] : _imResponse$data8$fai;
      var res = [];
      if (friendGroup && friendGroup.userIDList) {
        friendGroup.userIDList.forEach(function (userID) {
          res.push({
            friend_result_identifier: userID,
            friend_result_code: 0,
            friend_result_desc: 'success'
          });
        });
      }
      failureUserIDList.forEach(function (item) {
        res.push({
          friend_result_identifier: item.userID,
          friend_result_code: item.code,
          friend_result_desc: item.message
        });
      });
      var buf = utils.getStrBuf(res);
      Module['dynCall_viiii'](cb, imResponse.code, '', buf, user_data);
    }).catch(function (imError) {
      console.warn('getFriendGroupInfo error:', imError); 
    });
  },

  TIMFriendshipGetFriendGroupList: function TIMFriendshipGetFriendGroupList(json_get_friend_group_list_param, cb, user_data) {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    // List<FriendGroupInfo>
    var param = JSON.parse(UTF8ToString(json_get_friend_group_list_param));
    var promise = tim.getFriendGroupList();
    promise.then(function (imResponse) {
      var friendGroupList = imResponse.data;
      var res = [];
      if (friendGroupList) {
        friendGroupList.forEach(function (friendGroup) {
          if (param.length === 0 || param.includes(friendGroup.name)) {
            res.push({
              friend_group_info_name: friendGroup.name,
              friend_group_info_count: friendGroup.count,
              friend_group_info_identifier_array: friendGroup.userIDList
            });
          }
        });
      }
      var buf = utils.getStrBuf(res);
      Module['dynCall_viiii'](cb, imResponse.code, '', buf, user_data);
    }).catch(function (imError) {
      console.warn('getFriendGroupList error:', imError); 
    });
  },

  TIMFriendshipModifyFriendGroup: function TIMFriendshipModifyFriendGroup(json_modify_friend_group_param, cb, user_data) {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    // List<FriendResult>
    var param = JSON.parse(UTF8ToString(json_modify_friend_group_param));
    var changeNamePromise;
    var promises = [];
    var deleteIDs = param.friendship_modify_friend_group_param_delete_identifier_array;
    if (deleteIDs) {
      promises.push(tim.removeFromFriendGroup({
        name: param.friendship_modify_friend_group_param_name,
        userIDList: deleteIDs
      }));
    } else {
      deleteIDs = [];
    }
    var addIDs = param.friendship_modify_friend_group_param_add_identifier_array;
    if (addIDs) {
      promises.push(tim.addToFriendGroup({
        name: param.friendship_modify_friend_group_param_name,
        userIDList: addIDs
      }));
    } else {
      [], _readOnlyError("addIDs");
    }
    Promise.all(promises).then(function (imResponses) {
      if (param.friendship_modify_friend_group_param_new_name) {
        changeNamePromise = tim.renameFriendGroup({
          oldName: param.friendship_modify_friend_group_param_name,
          newName: param.friendship_modify_friend_group_param_new_name
        });
        changeNamePromise.then(function (imResponse) {}).catch(function (imError) {
          console.warn('renameFriendGroup error:', imError);
        });
      }
      var res = [];
      var failedUserID = new Set();
      for (var i = 0; i < imResponses.length; i++) {
        var friendGroup = imResponses[i].data.friendGroup;
        var failureUserIDList = imResponses[i].data.failureUserIDList;
        failureUserIDList.forEach(function (item) {
          failedUserID.add(item.userID);
          res.push({
            friend_result_identifier: item.userID,
            friend_result_code: item.code,
            friend_result_desc: item.message
          });
        });
      }
      addIDs.concat(deleteIDs).forEach(function (userID) {
        if (!failedUserID.has(userID)) {
          res.push({
            friend_result_identifier: userID,
            friend_result_code: 0,
            friend_result_desc: ''
          });
        }
      });
      var buf = utils.getStrBuf(res);
      Module['dynCall_viiii'](cb, 0, '', buf, user_data);
    }).catch(function (imError) {
      console.warn('TIMFriendshipModifyFriendGroup error:', imError);
    });
  },

  TIMFriendshipDeleteFriendGroup: function TIMFriendshipDeleteFriendGroup(json_delete_friend_group_param, cb, user_data) {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    var param = JSON.parse(UTF8ToString(json_delete_friend_group_param));
    var promise = tim.deleteFriendGroup({
      name: param[0]
    });
    promise.then(function (imResponse) {
      console.log(imResponse.data); 
      Module['dynCall_viiii'](cb, imResponse.code, '', '', user_data);
    }).catch(function (imError) {
      console.warn('deleteFriendGroup error:', imError);
    });
  },

  TIMFriendshipAddToBlackList: function TIMFriendshipAddToBlackList(json_add_to_blacklist_param, cb, user_data) {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    // List<FriendResult>
    var param = JSON.parse(UTF8ToString(json_add_to_blacklist_param));
    var promise = tim.addToBlacklist({
      userIDList: param
    }); 
    promise.then(function (imResponse) {
      console.log(imResponse.data); 
      var res = imResponse.data.map(function (userID) {
        return {
          friend_result_identifier: userID,
          friend_result_code: 0,
          friend_result_desc: 'success'
        };
      });
      var buf = utils.getStrBuf(res);
      Module['dynCall_viiii'](cb, imResponse.code, '', buf, user_data);
    }).catch(function (imError) {
      console.warn('addToBlacklist error:', imError); 
    });
  },

  TIMFriendshipGetBlackList: function TIMFriendshipGetBlackList(cb, user_data) {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    // List<FriendProfile>
    var promise = tim.getBlacklist();
    promise.then(function (imResponse) {
      console.log(imResponse.data); 
      var res = imResponse.data.map(function (userID) {
        return {
          friend_profile_identifier: userID
        };
      });
      store.blackList = imResponse.data;
      if (cb) {
        var buf = utils.getStrBuf(res);
        Module['dynCall_viiii'](cb, imResponse.code, '', buf, user_data);
      }
    }).catch(function (imError) {
      console.warn('getBlacklist error:', imError);
    });
  },

  TIMFriendshipDeleteFromBlackList: function TIMFriendshipDeleteFromBlackList(json_delete_from_blacklist_param, cb, user_data) {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    // List<FriendResult>
    var param = JSON.parse(UTF8ToString(json_delete_from_blacklist_param));
    var promise = tim.removeFromBlacklist({
      userIDList: param
    });
    promise.then(function (imResponse) {
      console.log(imResponse.data);
      var res = imResponse.data.map(function (userID) {
        return {
          friend_result_identifier: userID,
          friend_result_code: 0,
          friend_result_desc: 'success'
        };
      });
      var buf = utils.getStrBuf(res);
      Module['dynCall_viiii'](cb, imResponse.code, '', buf, user_data);
    }).catch(function (imError) {
      console.warn('removeFromBlacklist error:', imError);
    });
  },

  TIMFriendshipGetPendencyList: function TIMFriendshipGetPendencyList(json_get_pendency_list_param, cb, user_data) {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    // PendencyPage
    var promise = tim.getFriendApplicationList();
    promise.then(function (imResponse) {
      var _imResponse$data9 = imResponse.data,
        friendApplicationList = _imResponse$data9.friendApplicationList,
        unreadCount = _imResponse$data9.unreadCount;
      var applications = friendApplicationList.map(function (friendApplication) {
        return converter.toFriendAddPendencyInfo(friendApplication);
      });
      store.friendApplicationList = friendApplicationList;
      if (cb) {
        var res = {
          pendency_page_unread_num: unreadCount,
          pendency_page_pendency_info_array: applications
        };
        var buf = utils.getStrBuf(res);
        Module['dynCall_viiii'](cb, imResponse.code, '', buf, user_data);
      }
    }).catch(function (imError) {
      console.warn('getFriendApplicationList error:', imError);
    });
  },
  TIMFriendshipDeletePendency: function TIMFriendshipDeletePendency(json_delete_pendency_param, cb, user_data) {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    // List<FriendResult>
    var param = JSON.parse(UTF8ToString(json_delete_pendency_param));
    var type = converter.fromFriendPendencyType(param.friendship_delete_pendency_param_type);
    var promises = param.friendship_delete_pendency_param_identifier_array.map(function (userID) {
      return tim.deleteFriendApplication({
        userID: userID,
        type: type
      });
    });
    Promise.all(promises).then(function (imResponses) {
      var res = imResponses.map(function (imResponse, idx) {
        return {
          friend_result_identifier: param.friendship_delete_pendency_param_identifier_array[idx],
          friend_result_code: imResponse.code,
          friend_result_desc: ''
        };
      });
      var buf = utils.getStrBuf(res);
      Module['dynCall_viiii'](cb, 0, '', buf, user_data);
    }).catch(function (imError) {
      console.warn('deleteFriendApplication error:', imError);
    });
  },
  TIMFriendshipReportPendencyReaded: function TIMFriendshipReportPendencyReaded(time_stamp, un, cb, user_data) {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    // time_stamp is uint64, and here js uses 2 numbers for it.
    var promise = tim.setFriendApplicationRead();
    promise.then(function (imResponse) {
      Module['dynCall_viiii'](cb, 0, '', '', user_data);
    }).catch(function (imError) {
      console.warn('setFriendApplicationRead error:', imError);
    });
  },
  TIMFriendshipSearchFriends: function TIMFriendshipSearchFriends() {
    utils._handleNotImplemented();
  },
  TIMFriendshipGetFriendsInfo: function TIMFriendshipGetFriendsInfo(json_get_friends_info_param, cb, user_data) {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    // List<FriendInfoGetResult>
    var ids = JSON.parse(UTF8ToString(json_get_friends_info_param));
    var promise = tim.getFriendProfile({
      userIDList: ids
    });
    promise.then(function (imResponse) {
      var _imResponse$data10 = imResponse.data,
        friendList = _imResponse$data10.friendList,
        failureUserIDList = _imResponse$data10.failureUserIDList;
      var res = [];
      friendList.forEach(function (friend) {
        res.push({
          friendship_friend_info_get_result_identifier: friend.userID,
          friendship_friend_info_get_result_error_code: 0,
          friendship_friend_info_get_result_error_message: '',
          friendship_friend_info_get_result_field_info: converter.toFriendProfile(friend)
        });
      });
      failureUserIDList.forEach(function (item) {
        res.push({
          friendship_friend_info_get_result_identifier: item.userID,
          friendship_friend_info_get_result_error_code: item.code,
          friendship_friend_info_get_result_error_message: item.message
        });
      });
      var buf = utils.getStrBuf(res);
      Module['dynCall_viiii'](cb, imResponse.code, '', buf, user_data);
    }).catch(function (imError) {
      console.warn('getFriendProfile error:', imError); 
    });
  },

  callExperimentalAPI: function callExperimentalAPI() {
    utils._handleNotImplemented();
  },
  TIMMsgSendMessageReadReceipts: function TIMMsgSendMessageReadReceipts(json_msg_array, cb, user_data) {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    var param = JSON.parse(UTF8ToString(json_msg_array));
    var msgList = [];
    param.forEach(function (msg) {
      var msgJS = tim.findMessage(msg.message_msg_id);
      if (msgJS) {
        msgList.push(msgJS);
      }
    });
    tim.sendMessageReadReceipt(msgList).then(function () {
      Module['dynCall_viiii'](cb, 0, '', '', user_data);
    }).catch(function (imError) {
      console.warn('sendMessageReadReceipt error:', imError); 
    });
  },

  TIMMsgSetMessageExtensions: function TIMMsgSetMessageExtensions() {
    utils._handleNotImplemented();
  },
  TIMMsgGetMessageExtensions: function TIMMsgGetMessageExtensions() {
    utils._handleNotImplemented();
  },
  TIMMsgDeleteMessageExtensions: function TIMMsgDeleteMessageExtensions() {
    utils._handleNotImplemented();
  },

  TIMMsgGetMessageReadReceipts: function TIMMsgGetMessageReadReceipts(json_msg_array, cb, user_data) {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    // List<MessageReceipt>
    var param = JSON.parse(UTF8ToString(json_msg_array));
    var msgList = [];
    param.forEach(function (msg) {
      var msgJS = tim.findMessage(msg.message_msg_id);
      if (msgJS) {
        msgList.push(msgJS);
      }
    });
    tim.getMessageReadReceiptList(msgList).then(function (imResponse) {
      var messageList = imResponse.data.messageList; 
      var res = messageList.map(function (msg) {
        return converter.toMessageReceipt(msg);
      });
      var buf = utils.getStrBuf(res);
      Module['dynCall_viiii'](cb, imResponse.code, '', buf, user_data);
    }).catch(function (imError) {
    });
  },
  TIMMsgGetGroupMessageReadMemberList: function TIMMsgGetGroupMessageReadMemberList() {
    utils._handleNotImplemented();
  },
  TIMMsgGetGroupMessageReadMemberListWeb: function TIMMsgGetGroupMessageReadMemberListWeb(json_msg, filter, next_seq, count, cb, user_data) {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    // List<GroupMemberInfo>
    var param = JSON.parse(UTF8ToString(json_msg));
    var msg = tim.findMessage(param.message_msg_id);
    var promise = tim.getGroupMessageReadMemberList({
      message: msg,
      filter: filter,
      cursor: UTF8ToString(next_seq),
      count: count
    });
    promise.then(function (imResponse) {
      var _imResponse$data11 = imResponse.data,
        isCompleted = _imResponse$data11.isCompleted,
        cursor = _imResponse$data11.cursor,
        messageID = _imResponse$data11.messageID,
        _imResponse$data11$re = _imResponse$data11.readUserIDList,
        readUserIDList = _imResponse$data11$re === void 0 ? [] : _imResponse$data11$re,
        _imResponse$data11$un = _imResponse$data11.unreadUserIDList,
        unreadUserIDList = _imResponse$data11$un === void 0 ? [] : _imResponse$data11$un;
      var restList = filter === 0 ? readUserIDList : unreadUserIDList;
      if (restList.length < 1) {
        var buf = utils.getStrBuf([]);
        Module['dynCall_viiii'](cb, buf, utils.getStrBuf(cursor), isCompleted, user_data);
        return;
      }
      var groupID = param.message_conv_id.split('GROUP')[1];
      var options = {
        groupID: groupID,
        userIDList: restList
      };
      var promise = tim.getGroupMemberProfile(options);
      promise.then(function (imResponse) {
        var res = [];
        if (imResponse.data.memberList) {
          res = imResponse.data.memberList.map(function (member) {
            return converter.toGroupMemberInfo(member, groupID);
          });
        }
        var buf = utils.getStrBuf(res);
        Module['dynCall_viiii'](cb, buf, utils.getStrBuf(cursor), isCompleted, user_data);
      }).catch(function (imError) {
        console.warn('getGroupMemberProfile error:', imError);
      });
    }).catch(function (imError) {
      console.warn('getGroupMessageReadMemberList: ', imError);
    });
  },
  TIMAddRecvNewMsgCallback: function TIMAddRecvNewMsgCallback(cb, user_data) {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    if (!cb) {
      tim.off(TIM.EVENT.MESSAGE_RECEIVED, callback.onMessageReceived);
      return;
    }
    var onMessageReceived = function onMessageReceived(event) {
      var res = event.data.map(function (msg) {
        return converter.toMessage(msg);
      });
      var buf = utils.getStrBuf(res);
      Module['dynCall_vii'](cb, buf, user_data);
    };
    callback.onMessageReceived = onMessageReceived;
    tim.on(TIM.EVENT.MESSAGE_RECEIVED, callback.onMessageReceived);
  },
  TIMRemoveRecvNewMsgCallback: function TIMRemoveRecvNewMsgCallback() {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    tim.off(TIM.EVENT.MESSAGE_RECEIVED, callback.onMessageReceived);
  },
  TIMSetMsgExtensionsChangedCallback: function TIMSetMsgExtensionsChangedCallback () {
    utils._handleNotImplemented();
  },
  TIMSetMsgExtensionsDeletedCallback: function TIMSetMsgExtensionsDeletedCallback () {
    utils._handleNotImplemented();
  },
  TIMSetMsgReadedReceiptCallback: function TIMSetMsgReadedReceiptCallback(cb, user_data) {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    if (!cb) {
      tim.off(TIM.EVENT.MESSAGE_READ_RECEIPT_RECEIVED, callback.onMessageReadReceiptReceived);
      return;
    }
    var onMessageReadReceiptReceived = function onMessageReadReceiptReceived(event) {
      // List<MessageReceipt>
      var readReceiptInfoList = event.data ? event.data : [];
      var res = [];
      readReceiptInfoList.forEach(function (item) {
        // const { groupID, userID, messageID, readCount, unreadCount, isPeerRead } = item;
        var message = tim.findMessage(item.messageID);
        res.push({
          msg_receipt_conv_id: message.conversationID,
          msg_receipt_conv_type: converter.toConvType(message.conversationType),
          msg_receipt_time_stamp: message.time,
          msg_receipt_msg_id: item.messageID,
          msg_receipt_is_peer_read: item.isPeerRead,
          msg_receipt_read_count: item.readCount,
          msg_receipt_unread_count: item.unreadCount
        });
      });
      var buf = utils.getStrBuf(res);
      Module['dynCall_vii'](cb, buf, user_data);
    };
    callback.onMessageReadReceiptReceived = onMessageReadReceiptReceived;
    tim.on(TIM.EVENT.MESSAGE_READ_RECEIPT_RECEIVED, callback.onMessageReadReceiptReceived);
  },
  TIMSetMsgRevokeCallback: function TIMSetMsgRevokeCallback(cb, user_data) {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    if (!cb) {
      tim.off(TIM.EVENT.MESSAGE_REVOKED, callback.onMessageRevoked);
      return;
    }
    var onMessageRevoked = function onMessageRevoked(event) {
      // List<MsgLocator>
      var res = event.data.map(function (message) {
        return converter.toMsgLocator(message);
      });
      var buf = utils.getStrBuf(res);
      Module['dynCall_vii'](cb, buf, user_data);
    };
    callback.onMessageRevoked = onMessageRevoked;
    tim.on(TIM.EVENT.MESSAGE_REVOKED, callback.onMessageRevoked);
  },
  TIMSetMsgElemUploadProgressCallback: function TIMSetMsgElemUploadProgressCallback(cb, user_data) {
    utils._handleNotImplemented();
  },
  TIMSetGroupTipsEventCallback: function TIMSetGroupTipsEventCallback(cb, user_data) {
    utils._handleNotImplemented();
  },
  TIMSetGroupAttributeChangedCallback: function TIMSetGroupAttributeChangedCallback(cb, user_data) {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    if (!cb) {
      tim.off(TIM.EVENT.GROUP_ATTRIBUTES_UPDATED, callback.onGroupAttributesUpdated);
      return;
    }
    var onGroupAttributesUpdated = function onGroupAttributesUpdated(event) {
      var groupID = event.data.groupID;
      var groupAttributes = event.data.groupAttributes ? event.data.groupAttributes : [];
      var group_id = utils.genStrBuf(groupID);
      var res = Object.keys(groupAttributes).map(function (key) {
        return {
          group_atrribute_key: key,
          group_atrribute_value: groupAttributes[key]
        };
      });
      var buf = utils.getStrBuf(res);
      Module['dynCall_viii'](cb, group_id, buf, user_data);
    };
    callback.onGroupAttributesUpdated = onGroupAttributesUpdated;
    tim.on(TIM.EVENT.GROUP_ATTRIBUTES_UPDATED, callback.onGroupAttributesUpdated);
  },
  TIMSetConvEventCallback: function TIMSetConvEventCallback(cb, user_data) {
    utils._handleNotImplemented();
  },
  TIMSetConvConversationGroupCreatedCallback: function TIMSetConvConversationGroupCreatedCallback(cb, user_data) {
    utils._handleNotImplemented();
  },
  TIMSetConvConversationGroupDeletedCallback: function TIMSetConvConversationGroupDeletedCallback(cb, user_data) {
    utils._handleNotImplemented();
  },
  TIMSetConvConversationGroupNameChangedCallback: function TIMSetConvConversationGroupNameChangedCallback(cb, user_data) {
    utils._handleNotImplemented();
  },
  TIMSetConvConversationsAddedToGroupCallback: function TIMSetConvConversationsAddedToGroupCallback(cb, user_data) {
    utils._handleNotImplemented();
  },
  TIMSetConvConversationsDeletedFromGroupCallback: function TIMSetConvConversationsDeletedFromGroupCallback(cb, user_data) {
    utils._handleNotImplemented();
  },
  TIMSetConvTotalUnreadMessageCountChangedCallback: function TIMSetConvTotalUnreadMessageCountChangedCallback(cb, user_data) {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    if (!TIM.EVENT.TOTAL_UNREAD_MESSAGE_COUNT_UPDATED) {
      return;
    }
    if (!cb) {
      tim.off(TIM.EVENT.TOTAL_UNREAD_MESSAGE_COUNT_UPDATED, callback.onTotalUnreadMessageCountUpdated);
      return;
    }
    var onTotalUnreadMessageCountUpdated = function onTotalUnreadMessageCountUpdated(event) {
      console.log(event.data);
      Module['dynCall_vii'](cb, Number(event.data), user_data);
    };
    callback.onTotalUnreadMessageCountUpdated = onTotalUnreadMessageCountUpdated;
    tim.on(TIM.EVENT.TOTAL_UNREAD_MESSAGE_COUNT_UPDATED, callback.onTotalUnreadMessageCountUpdated);
  },
  TIMSetNetworkStatusListenerCallback: function TIMSetNetworkStatusListenerCallback(cb, user_data) {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    if (!cb) {
      tim.off(TIM.EVENT.NET_STATE_CHANGE, callback.onNetStateChange);
      return;
    }
    var onNetStateChange = function onNetStateChange(event) {
      var state = converter.toNetworkStatus(event.data.state);
      Module['dynCall_viiii'](cb, state, 0, '', user_data);
    };
    callback.onNetStateChange = onNetStateChange;
    tim.on(TIM.EVENT.NET_STATE_CHANGE, callback.onNetStateChange);
  },
  TIMSetKickedOfflineCallback: function TIMSetKickedOfflineCallback(cb, user_data) {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    if (!cb) {
      callback._onKickedOut = null;
      if (!callback._onSigExpired) {
        tim.off(TIM.EVENT.KICKED_OUT, callback.onKicked);
      }
      return;
    }
    var _onKickedOut = function _onKickedOut(event) {
      console.log(event.data.type);
      if (event.data.type !== TIM.TYPES.KICKED_OUT_USERSIG_EXPIRED) {
        Module['dynCall_vi'](cb, user_data);
      }
    };
    callback._onKickedOut = _onKickedOut;
    tim.on(TIM.EVENT.KICKED_OUT, callback.onKicked);
  },
  TIMSetUserSigExpiredCallback: function TIMSetUserSigExpiredCallback(cb, user_data) {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    if (!cb) {
      callback._onSigExpired = null;
      if (!callback._onKickedOut) {
        tim.off(TIM.EVENT.KICKED_OUT, callback.onKicked);
      }
      return;
    }
    var _onSigExpired = function _onSigExpired(event) {
      console.log(event.data.type);
      if (event.data.type === TIM.TYPES.KICKED_OUT_USERSIG_EXPIRED) {
        Module['dynCall_vi'](cb, user_data);
      }
    };
    callback._onSigExpired = _onSigExpired;
    tim.on(TIM.EVENT.KICKED_OUT, callback.onKicked);
  },
  TIMSetOnAddFriendCallback: function TIMSetOnAddFriendCallback(cb, user_data) {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    if (!cb) {
      callback._onAddFriend = null;
      if (!callback._onDeleteFriend) {
        tim.off(TIM.EVENT.FRIEND_LIST_UPDATED, callback.onFriendListUpdated);
      }
      return;
    }
    var _onAddFriend = function _onAddFriend(userIDs) {
      var buf = utils.getStrBuf(userIDs);
      Module['dynCall_vii'](cb, buf, user_data);
    };
    callback._onAddFriend = _onAddFriend;
    tim.on(TIM.EVENT.FRIEND_LIST_UPDATED, callback.onFriendListUpdated);
  },
  TIMSetOnDeleteFriendCallback: function TIMSetOnDeleteFriendCallback(cb, user_data) {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    if (!cb) {
      callback._onDeleteFriend = null;
      if (!callback._onAddFriend) {
        tim.off(TIM.EVENT.FRIEND_LIST_UPDATED, callback.onFriendListUpdated);
      }
      return;
    }
    var _onDeleteFriend = function _onDeleteFriend(userIDs) {
      var buf = utils.getStrBuf(userIDs);
      Module['dynCall_vii'](cb, buf, user_data);
    };
    callback._onDeleteFriend = _onDeleteFriend;
    tim.on(TIM.EVENT.FRIEND_LIST_UPDATED, callback.onFriendListUpdated);
  },
  TIMSetUpdateFriendProfileCallback: function TIMSetUpdateFriendProfileCallback(cb, user_data) {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    // List<FriendProfileItem>
    if (!cb) {
      callback._onFriendProfileUpdated = null;
      if (!callback._onSelfInfoUpdated) {
        tim.off(TIM.EVENT.PROFILE_UPDATED, callback.onProfileUpdated);
      }
      return;
    }
    var _onFriendProfileUpdated = function _onFriendProfileUpdated(event) {
      console.log(event.data); 
      var changedProfiles = event.data ? event.data : [];
      var promise = tim.getFriendList();
      promise.then(function (imResponse) {
        var friendList = changedProfiles.filter(function (profile) {
          return imResponse.data.findIndex(function (friend) {
            return friend.userID === profile.userID;
          }) > -1;
        });
        var res = friendList.map(function (friend) {
          return converter.toFriendProfileItem(friend);
        });
        var buf = utils.getStrBuf(res);
        Module['dynCall_vii'](cb, buf, user_data);
      }).catch(function (imError) {
        console.warn('getFriendList error:', imError); 
      });
    };

    callback._onFriendProfileUpdated = _onFriendProfileUpdated;
    tim.on(TIM.EVENT.PROFILE_UPDATED, callback.onProfileUpdated);
  },
  TIMSetFriendAddRequestCallback: function TIMSetFriendAddRequestCallback(cb, user_data) {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    // FRIEND_APPLICATION_LIST_UPDATED
    if (!cb) {
      callback._onFriendAddRequest = null;
      if (!callback._onFriendApplicationListDeleted) {
        tim.off(TIM.EVENT.FRIEND_APPLICATION_LIST_UPDATED, callback.onFriendApplicationListUpdated);
      }
      return;
    }
    var _onFriendAddRequest = function _onFriendAddRequest(pendencies) {
      var buf = utils.getStrBuf(pendencies);
      Module['dynCall_vii'](cb, buf, user_data);
    };
    callback._onFriendAddRequest = _onFriendAddRequest;
    tim.on(TIM.EVENT.FRIEND_APPLICATION_LIST_UPDATED, callback.onFriendApplicationListUpdated);
  },
  TIMSetFriendApplicationListDeletedCallback: function TIMSetFriendApplicationListDeletedCallback(cb, user_data) {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    // FRIEND_APPLICATION_LIST_UPDATED
    if (!cb) {
      callback._onFriendApplicationListDeleted = null;
      if (!callback._onFriendAddRequest) {
        tim.off(TIM.EVENT.FRIEND_APPLICATION_LIST_UPDATED, callback.onFriendApplicationListUpdated);
      }
      return;
    }
    var _onFriendApplicationListDeleted = function _onFriendApplicationListDeleted(userIDs) {
      var buf = utils.getStrBuf(userIDs);
      Module['dynCall_vii'](cb, buf, user_data);
    };
    callback._onFriendApplicationListDeleted = _onFriendApplicationListDeleted;
    tim.on(TIM.EVENT.FRIEND_APPLICATION_LIST_UPDATED, callback.onFriendApplicationListUpdated);
  },
  TIMSetFriendApplicationListReadCallback: function TIMSetFriendApplicationListReadCallback(cb, user_data) {
    utils._handleNotImplemented();
  },
  TIMSetFriendBlackListAddedCallback: function TIMSetFriendBlackListAddedCallback(cb, user_data) {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    if (!cb) {
      callback._onFriendBlackListAdded = null;
      if (!callback._onFriendBlackListDeleted) {
        tim.off(TIM.EVENT.BLACKLIST_UPDATED, callback.onBlacklistUpdated);
      }
      return;
    }
    var _onFriendBlackListAdded = function _onFriendBlackListAdded(userIDs) {
      var buf = utils.getStrBuf(userIDs);
      Module['dynCall_vii'](cb, buf, user_data);
    };
    callback._onFriendBlackListAdded = _onFriendBlackListAdded;
    tim.on(TIM.EVENT.BLACKLIST_UPDATED, callback.onBlacklistUpdated);
  },
  TIMSetFriendBlackListDeletedCallback: function TIMSetFriendBlackListDeletedCallback(cb, user_data) {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    if (!cb) {
      callback._onFriendBlackListDeleted = null;
      if (!callback._onFriendBlackListAdded) {
        tim.off(TIM.EVENT.BLACKLIST_UPDATED, callback.onBlacklistUpdated);
      }
      return;
    }
    var _onFriendBlackListDeleted = function _onFriendBlackListDeleted(userIDs) {
      var buf = utils.getStrBuf(userIDs);
      Module['dynCall_vii'](cb, buf, user_data);
    };
    callback._onFriendBlackListDeleted = _onFriendBlackListDeleted;
    tim.on(TIM.EVENT.BLACKLIST_UPDATED, callback.onBlacklistUpdated);
  },
  TIMSetLogCallback: function TIMSetLogCallback(cb, user_data) {
    utils._handleNotImplemented();
  },
  TIMSetMsgUpdateCallback: function TIMSetMsgUpdateCallback(cb, user_data) {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    if (!cb) {
      tim.off(TIM.EVENT.MESSAGE_MODIFIED, callback.onMessageModified);
      return;
    }
    var onMessageModified = function onMessageModified(event) {
      var msgList = event.data ? event.data : [];
      var res = msgList.map(function (msg) {
        return converter.toMessage(msg);
      });
      var buf = utils.getStrBuf(res);
      Module['dynCall_vii'](cb, buf, user_data);
    };
    callback.onMessageModified = onMessageModified;
    tim.on(TIM.EVENT.MESSAGE_MODIFIED, callback.onMessageModified);
  },
  TIMSetGroupTopicCreatedCallback: function TIMSetGroupTopicCreatedCallback(cb, user_data) {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    if (!cb) {
      tim.off(TIM.EVENT.TOPIC_CREATED, callback.onTopicCreated);
      return;
    }
    var onTopicCreated = function onTopicCreated(event) {
      var groupID = event.data.groupID; 
      var topicID = event.data.topicID; 
      var groupIDBuf = utils.getStrBuf(groupID);
      var topicIDBuf = utils.getStrBuf(topicID);
      Module['dynCall_viii'](cb, groupIDBuf, topicIDBuf, user_data);
    };
    callback.onTopicCreated = onTopicCreated;
    tim.on(TIM.EVENT.TOPIC_CREATED, callback.onTopicCreated);
  },
  TIMSetGroupTopicDeletedCallback: function TIMSetGroupTopicDeletedCallback(cb, user_data) {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    if (!cb) {
      tim.off(TIM.EVENT.TOPIC_DELETED, callback.onTopicDeleted);
      return;
    }
    var onTopicDeleted = function onTopicDeleted(event) {
      var groupID = event.data.groupID;
      var topicIDList = event.data.topicIDList ? event.data.topicIDList : [];
      var groupIDBuf = utils.getStrBuf(groupID);
      var topicIDListBuf = utils.getStrBuf(topicIDList);
      Module['dynCall_viii'](cb, groupIDBuf, topicIDListBuf, user_data);
    };
    callback.onTopicDeleted = onTopicDeleted;
    tim.on(TIM.EVENT.TOPIC_DELETED, callback.onTopicDeleted);
  },
  TIMSetGroupTopicChangedCallback: function TIMSetGroupTopicChangedCallback(cb, user_data) {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    if (!cb) {
      tim.off(TIM.EVENT.TOPIC_UPDATED, callback.onTopicUpdated);
      return;
    }
    var onTopicUpdated = function onTopicUpdated(event) {
      var groupID = event.data.groupID;
      var topic = converter.toGroupTopicInfo(event.data.topic); 
      var groupIDBuf = utils.getStrBuf(groupID);
      var topicBuf = utils.getStrBuf(topic);
      Module['dynCall_viii'](cb, groupIDBuf, topicBuf, user_data);
    };
    callback.onTopicUpdated = onTopicUpdated;
    tim.on(TIM.EVENT.TOPIC_UPDATED, callback.onTopicUpdated);
  },
  TIMSetSelfInfoUpdatedCallback: function TIMSetSelfInfoUpdatedCallback(cb, user_data) {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    if (!cb) {
      callback._onSelfInfoUpdated = null;
      if (!callback._onFriendProfileUpdated) {
        tim.off(TIM.EVENT.PROFILE_UPDATED, callback.onProfileUpdated);
      }
      return;
    }
    var _onSelfInfoUpdated = function _onSelfInfoUpdated(event) {
      console.log(event.data); 
      var changedProfiles = event.data ? event.data : [];
      var promise = tim.getMyProfile();
      promise.then(function (imResponse) {
        var my = imResponse.data;
        var changedProfile = changedProfiles.find(function (profile) {
          return profile.userID === my.userID;
        });
        if (changedProfile) {
          var res = converter.toUserProfile(changedProfile);
          var buf = utils.getStrBuf(res);
          Module['dynCall_vii'](cb, buf, user_data);
        }
      }).catch(function (imError) {
        console.warn('getMyProfile error:', imError); 
      });
    };

    callback._onSelfInfoUpdated = _onSelfInfoUpdated;
    tim.on(TIM.EVENT.PROFILE_UPDATED, callback.onProfileUpdated);
  },
  TIMSetUserStatusChangedCallback: function TIMSetUserStatusChangedCallback(cb, user_data) {
    if (!utils.isInit) {
      utils._handleNotLoggedIn();
      return;
    }
    if (!cb) {
      tim.off(TIM.EVENT.USER_STATUS_UPDATED, callback.onUserStatusUpdated);
      return;
    }
    var onUserStatusUpdated = function onUserStatusUpdated(event) {
      var userStatusList = event.data ? event.data : [];
      var res = userStatusList.map(function (item) {
        return {
          user_status_identifier: item.userID,
          user_status_status_type: converter.toUserStatusType(item.statusType),
          user_status_custom_status: item.customStatus
        };
      });
      var buf = utils.getStrBuf(res);
      Module['dynCall_vii'](cb, buf, user_data);
    };
    callback.onUserStatusUpdated = onUserStatusUpdated;
    tim.on(TIM.EVENT.USER_STATUS_UPDATED, callback.onUserStatusUpdated);
  }
};
autoAddDeps(ImSDKPlugin, '$tim');
autoAddDeps(ImSDKPlugin, '$store');
autoAddDeps(ImSDKPlugin, '$utils');
autoAddDeps(ImSDKPlugin, '$converter');
autoAddDeps(ImSDKPlugin, '$callback');
mergeInto(LibraryManager.library, ImSDKPlugin);