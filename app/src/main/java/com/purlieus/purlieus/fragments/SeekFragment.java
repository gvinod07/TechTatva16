package com.purlieus.purlieus.fragments;

import android.content.Context;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.LinearLayout;
import android.widget.Spinner;

import com.microsoft.windowsazure.mobileservices.MobileServiceClient;
import com.microsoft.windowsazure.mobileservices.http.ServiceFilterResponse;
import com.microsoft.windowsazure.mobileservices.table.TableOperationCallback;
import com.purlieus.purlieus.R;
//import com.purlieus.purlieus.models.BD_Donate;
import com.purlieus.purlieus.adapters.SeekAdapter;
import com.purlieus.purlieus.models.BD_Seek;

import java.net.MalformedURLException;
import java.util.ArrayList;
import java.util.List;

import static com.google.android.gms.analytics.internal.zzy.co;

/**
 * Created by anurag on 4/10/16.
 */
public class SeekFragment extends Fragment {

    private Spinner spinner;
    public static final String PROFILE_DATA="profile";
    private MobileServiceClient mClient;
    Context context;
    SeekAdapter seekAdapter;
    List<BD_Seek> seekResult = new ArrayList<BD_Seek>();
    BD_Seek item;

    public SeekFragment() {
    }

    @Nullable
    @Override
    public View onCreateView(LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_seek, container, false);

        spinner = (Spinner)view.findViewById(R.id.bg_spinner);
        String[] groups = {"O+", "O-", "A+", "A-", "B+", "B-", "AB+", "AB-"};
        ArrayAdapter<String> adapter = new ArrayAdapter<String>(getActivity(), android.R.layout.simple_spinner_dropdown_item, groups);
        spinner.setAdapter(adapter);

        item = new BD_Seek();

        try{
            mClient = new MobileServiceClient("https://purlieus.azurewebsites.net", getActivity());
        }
        catch(MalformedURLException e){
            e.printStackTrace();
        }

        spinner.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener(){
            @Override
            public void onItemSelected(AdapterView<?> parent, View view, int position, long id) {
                SharedPreferences.Editor editor = getActivity().getSharedPreferences(PROFILE_DATA, Context.MODE_PRIVATE).edit();
                editor.putString("bg_seek", parent.getItemAtPosition(position).toString());
                editor.apply();
                Log.d("Selected", parent.getItemAtPosition(position).toString());

                item.setName("Bhatnagar");
                item.setSex("M");
                item.setAge(12);
                item.setBloodGroup("O+");
                item.setEmail("abc@pqr.com");
                item.setContactNumber("9876543210");
                item.setLatitude("23N");
                item.setLongitude("35N");
                item.setUrgent(false);
                mClient.getTable(BD_Seek.class).insert(item, new TableOperationCallback<BD_Seek>() {
                @Override
                public void onCompleted(BD_Seek entity, Exception exception, ServiceFilterResponse response) {
                    if (exception == null) {
                    // Insert succeeded
                        Log.d("Seek", "successful");
                    } else {
                        exception.printStackTrace();
                        Log.d("Seek", "exception");
                    }
                }
                });

            }

            @Override
            public void onNothingSelected(AdapterView<?> parent) {

            }
        });

        for(int i=1; i<=5; i++){
            seekResult.add(item);
        }

        final RecyclerView usersRecyclerView = (RecyclerView)view.findViewById(R.id.seek_recycler_view);
        seekAdapter = new SeekAdapter(getActivity(), seekResult);
        usersRecyclerView.setAdapter(seekAdapter);
        usersRecyclerView.setLayoutManager(new LinearLayoutManager(getActivity()));

        //Continue Button OnClickListener
        final LinearLayout linearLayout = (LinearLayout) view.findViewById(R.id.seek_tab);

        Button contButton = (Button) view.findViewById(R.id.continueButton);
        contButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                //Log.d("TAG", "Entered On Click");
                usersRecyclerView.setVisibility(View.VISIBLE);
                linearLayout.setVisibility(View.GONE);

            }
        });
        //Log.d("TAG", "Exited click");
        return view;
    }
}
