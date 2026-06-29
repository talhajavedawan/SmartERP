using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErpChatService.Models;
using ErpChatService.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;
using ERP_BL.ChatManager;
using ERP_BL.Databases;

namespace ErpChatService.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    public class ChatServiceController : Controller
    {
        private readonly ErpChatService.Services.ChatService _ChatGroupService;

        public ChatServiceController(ChatService chatGroupService)
        {
            _ChatGroupService = chatGroupService;
        }
        
        [HttpGet("api/ServerTest/IsServerAvailable")]
        //[HttpGet(Name = "GetAllGroups")]
        public ActionResult<Boolean> IsServerAvailable() =>
            _ChatGroupService.IsServerAvailable();


        [HttpGet("api/ChatGroups/GetAllGroups")]
        //[HttpGet(Name = "GetAllGroups")]
        public ActionResult<List<ChatGroup>> GetChatGroups() =>
            _ChatGroupService.GetChatGroups();


        [HttpGet("api/ChatGroups/GetSingleGroup")]
        [ProducesResponseType(StatusCodes.Status200OK)] 
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ChatGroup> GetChatGroup(int id)
        {
            var _chatGroup = _ChatGroupService.GetChatGroup(id);

            if (_chatGroup == null)
            {
                return NotFound("Chat Group not found");
            }

            return _chatGroup;
        }

        //[HttpPost("api/ChatGroups/create")]
        [HttpPost]
        [Route("api/ChatGroups/Create")]
        public ActionResult<ChatGroup> CreateChatGroup([FromBody] ChatGroup chatGroup)
        {
            _ChatGroupService.CreateChatGroup(chatGroup);

            return chatGroup; //CreatedAtRoute("GetChatGroup", new { id = chatGroup.id.ToString() }, chatGroup);
        }

        [HttpPost]
        [Route("api/ChatGroups/Update")]
        public IActionResult UpdateChatGroup(int id, [FromBody] ChatGroup groupIn)
        {
            var chatGroup = _ChatGroupService.GetChatGroup(id);

            if (chatGroup == null)
            {
                return NotFound("Chat group not found");
            }

            _ChatGroupService.UpdateChatGroup(id, groupIn);

            return NoContent();
        }

        [HttpDelete]
        [Route("api/ChatGroups/Delete")]
        public IActionResult DeleteChatGroup(int id)
        {
            var _chatGroup = _ChatGroupService.GetChatGroup(id);

            if (_chatGroup == null)
            {
                return NotFound("Group not found");
            }

            _ChatGroupService.RemoveChatGroup(_chatGroup.id);

            return NoContent();
        }

        //======================== chat contacts ===============================
        [HttpGet("api/ChatUsers/GetAllContacts")]
        public ActionResult<List<ChatUser>> GetChatUsers() =>
           _ChatGroupService.GetChatUsers();


        [HttpGet("api/ChatUsers/GetSingleContact")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ChatUser> GetChatUser(int id)
        {
            var _ChatUser = _ChatGroupService.GetChatUser(id);

            if (_ChatUser == null)
            {
                return NotFound("Chat Group not found");
            }

            return _ChatUser;
        }

        //[HttpPost("api/ChatGroups/create")]
        [HttpPost]
        [Route("api/ChatUsers/Create")]
        public ActionResult<ChatUser> CreateChatUser([FromBody] ChatUser ChatUser)
        {
            _ChatGroupService.CreateChatUser(ChatUser);

            return ChatUser; //CreatedAtRoute("GetChatGroup", new { id = chatGroup.id.ToString() }, chatGroup);
        }

        [HttpPost]
        [Route("api/ChatUsers/Update")]
        public IActionResult UpdateChatUser(int id, [FromBody] ChatUser contactIn)
        {
            var contact = _ChatGroupService.GetChatUser(id);

            if (contact == null)
            {
                return NotFound("Chat contact not found");
            }

            _ChatGroupService.UpdateChatUser(id, contactIn);

            return NoContent();
        }

        [HttpDelete]
        [Route("api/ChatUsers/Delete")]
        public IActionResult DeleteChatUser(int id)
        {
            var _ChatUser = _ChatGroupService.GetChatUser(id);

            if (_ChatUser == null)
            {
                return NotFound("Group not found");
            }

            _ChatGroupService.RemoveChatUser(_ChatUser.employeeId);

            return NoContent();
        }

        // ================================= One2One Chat =======================

    }
}
