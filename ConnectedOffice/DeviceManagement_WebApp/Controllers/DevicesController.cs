﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DeviceManagement_WebApp.Data;
using DeviceManagement_WebApp.Models;
using DeviceManagement_WebApp.Repository;

namespace DeviceManagement_WebApp.Controllers
{
    public class DevicesController : Controller
    {
        private readonly ConnectedOfficeContext _context;
        private readonly IDeviceRepository _devicesRepository;
        


        public DevicesController(IDeviceRepository deviceRepositiory)
        {
            _devicesRepository = deviceRepositiory;
       
        }

        // GET: Devices
        public async Task<IActionResult> Index()
        {
            
           var connectedOfficeContext = _devicesRepository.GetAll(); ;
            
            return View(_devicesRepository.GetAll());
        }

        // GET: Devices/Details/5
       public async Task<IActionResult> Details(Guid? id)
        {

          
            if (id == null)
            {
                return NotFound();
            }

            var device = _devicesRepository.GetById(id);

            if (device == null)
            {
                return NotFound();
            }

            return View(device);
        }

        
        // GET: Devices/Create
        public IActionResult Create()
        {

            
            ViewData["CategoryId"] = new SelectList(_devicesRepository.GetSet<Category>(), "CategoryId", "CategoryName");
           ViewData["ZoneId"] = new SelectList(_devicesRepository.GetSet<Zone>(), "ZoneId", "ZoneName");
            return View();
        }

        // POST: Devices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DeviceId,DeviceName,CategoryId,ZoneId,Status,IsActive,DateCreated")] Device device)
        {
            device.DeviceId = Guid.NewGuid();
            _devicesRepository.Add(device);
           
            return RedirectToAction(nameof(Index));


        }

        // GET: Devices/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Device device = _devicesRepository.GetById(id);
            _devicesRepository.Update(device);
            if (device == null)
            {
                return NotFound();
            }

            
            ViewData["CategoryId"] = new SelectList(_devicesRepository.GetSet<Category>(), "CategoryId", "CategoryName", device.CategoryId);
            ViewData["ZoneId"] = new SelectList(_devicesRepository.GetSet<Zone>(), "ZoneId", "ZoneName", device.ZoneId);
            return View();
        }

        // POST: Devices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("DeviceId,DeviceName,CategoryId,ZoneId,Status,IsActive,DateCreated")] Device device)
        {
            if (id != device.DeviceId)
            {
                return NotFound();
            }
            try
            {
                _devicesRepository.Update(device);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeviceExists(device.DeviceId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));

        }

        // GET: Devices/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var device = _devicesRepository.GetById(id);
            _devicesRepository.Remove(device);
            
            if (device == null)
            {
                return NotFound();
            }

            return View(device);
        }

        // POST: Devices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            _devicesRepository.DeleteConfirmed(id);
            return  RedirectToAction(nameof(Index));
        }

        private bool DeviceExists(Guid id)
        {
            return _devicesRepository.Object_Exists(id);
        }
    }
}
