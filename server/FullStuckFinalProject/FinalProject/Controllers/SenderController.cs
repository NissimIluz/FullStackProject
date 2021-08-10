using MessengerContracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProjectContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Controllers
{
    [Route("api/[controller]/{action}")]
    [ApiController]
    public class SenderController : ControllerBase
    {
        IMessenger _messanger;
        ILogger<SenderController> _logger;
        public SenderController(IMessenger messenger, ILogger<SenderController> logger)
        {
            _messanger = messenger;
            _logger = logger;
        }


        // POST api/<SenderController>
        [HttpPost]
        public async Task Send(DocumentRequest messageRequest)
        {
            await _messanger.Send(messageRequest.ID, messageRequest.MessageBody);
        }
        // POST api/<SenderController>
        [HttpPost]
        public async Task Broadcast(MarkerRequest messageRequest)
        {
            await _messanger.Broadcast(messageRequest.ID, messageRequest.MessageBody);
        }


    }
}
