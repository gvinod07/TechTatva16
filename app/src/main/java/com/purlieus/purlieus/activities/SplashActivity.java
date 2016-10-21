package com.purlieus.purlieus.activities;

import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;
import android.os.Handler;
import android.support.annotation.Nullable;
import android.support.v7.app.AppCompatActivity;
import android.widget.ImageView;

import com.purlieus.purlieus.R;

/**
 * Created by Shaurya on 07-Oct-16.
 */

public class SplashActivity extends AppCompatActivity{

    private final int SPLASH_RUN_TIME=1000;
    private Activity activity;
    private ImageView purlieusLogo;

    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_splash);
        purlieusLogo = (ImageView)findViewById(R.id.splash_logo);


        activity = this;

        new Handler().postDelayed(new Runnable() {
            @Override
            public void run() {
                Intent intent = new Intent(activity, DetailsActivity.class);
                activity.startActivity(intent);
                activity.finish();
            }
        }, SPLASH_RUN_TIME);

    }
}
