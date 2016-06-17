// Copyright (c) 2015-present, Parse, LLC.  All rights reserved.  This source code is licensed under the BSD-style license found in the LICENSE file in the root directory of this source tree.  An additional grant of patent rights can be found in the PATENTS file in the same directory.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

using Foundation;
using UIKit;
using System.Threading.Tasks;
using CoreFoundation;

namespace Parse {
  public partial class ParseInstallation : ParseObject {
    /// <summary>
    /// The device token of the installation. Typically generated by APNS or GCM.
    /// </summary>
    [ParseFieldName("deviceToken")]
    public string DeviceToken {
      get { return GetProperty<string>(); }
      internal set {
        string deviceToken = value;
        // Validate deviceToken.
        Regex regex = new Regex("[0-9a-f]{64}");
        if (regex.IsMatch(deviceToken)) {
          SetProperty<string>(deviceToken);
        } else {
          throw new InvalidOperationException("Device token must match /[0-9a-f]{64}/.");
        }
      }
    }

    /// <summary>
    /// Sets the installation with APNS generated device token.
    /// </summary>
    /// <param name="deviceToken">The device token</param>
    public void SetDeviceTokenFromData(NSData deviceToken) {
      var unmanagedBytes = deviceToken.Bytes;
      byte[] bytes = new byte[deviceToken.Length];
      Marshal.Copy(unmanagedBytes, bytes, 0, bytes.Length);

      StringBuilder builder = new StringBuilder();
      foreach (var b in bytes) {
        builder.Append(b.ToString("x2"));
      }
      DeviceToken = builder.ToString();
    }

    /// <summary>
    /// iOS Badge.
    /// </summary>
    [ParseFieldName("badge")]
    public int Badge {
      get {
        if (CurrentInstallationController.IsCurrent(this)) {
          RunOnUIQueue(() =>
            SetProperty<int>((int)UIApplication.SharedApplication.ApplicationIconBadgeNumber)
          );
        }
        return GetProperty<int>();
      }
      set {
        int badge = value;
        // Set the UI, too
        RunOnUIQueue(() => {
          UIApplication.SharedApplication.ApplicationIconBadgeNumber = (nint)badge;
        });
        SetProperty<int>(badge);
      }
    }

    /// <summary>
    /// Runs (synchronously) the on user interface queue.
    ///
    /// Note that this could cause deadlocks, if the main thread is waiting on
    /// the background thread we're calling this from.
    ///
    /// However, Xamarin's native bindings always threw exceptions when calling
    /// into UIKit code from a background thread, so this at least helps us in
    /// some deadlock scenarios.
    /// </summary>
    /// <param name="action">Action.</param>
    private static void RunOnUIQueue(Action action) {
      if (NSThread.IsMain) {
        action();
      } else {
        DispatchQueue.MainQueue.DispatchSync(action);
      }
    }
  }
}
